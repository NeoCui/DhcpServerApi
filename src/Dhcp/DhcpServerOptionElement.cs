﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Dhcp.Native;

namespace Dhcp
{
    public abstract class DhcpServerOptionElement
    {
        public abstract DhcpServerOptionElementType Type { get; }

        public abstract object Value { get; }
        public abstract string ValueFormatted { get; }

        internal static IEnumerable<DhcpServerOptionElement> ReadNativeElements(DHCP_OPTION_DATA elementArray)
        {
            foreach (var element in elementArray.Elements)
                yield return ReadNative(element);
        }

        internal static DHCP_OPTION_DATA_Managed WriteNative(IEnumerable<DhcpServerOptionElement> elements)
        {
            return new DHCP_OPTION_DATA_Managed(elements.Select(e => e.ToNative()).ToArray());
        }

        private static DhcpServerOptionElement ReadNative(DHCP_OPTION_DATA_ELEMENT element)
        {
            switch (element.OptionType)
            {
                case DHCP_OPTION_DATA_TYPE.DhcpByteOption:
                    return DhcpServerOptionElementByte.ReadNative(element);
                case DHCP_OPTION_DATA_TYPE.DhcpWordOption:
                    return DhcpServerOptionElementWord.ReadNative(element);
                case DHCP_OPTION_DATA_TYPE.DhcpDWordOption:
                    return DhcpServerOptionElementDWord.ReadNative(element);
                case DHCP_OPTION_DATA_TYPE.DhcpDWordDWordOption:
                    return DhcpServerOptionElementDWordDWord.ReadNative(element);
                case DHCP_OPTION_DATA_TYPE.DhcpIpAddressOption:
                    return DhcpServerOptionElementIpAddress.ReadNative(element);
                case DHCP_OPTION_DATA_TYPE.DhcpStringDataOption:
                    return DhcpServerOptionElementString.ReadNative(element);
                case DHCP_OPTION_DATA_TYPE.DhcpBinaryDataOption:
                case DHCP_OPTION_DATA_TYPE.DhcpEncapsulatedDataOption:
                    return DhcpServerOptionElementBinary.ReadNative(element);
                case DHCP_OPTION_DATA_TYPE.DhcpIpv6AddressOption:
                    return DhcpServerOptionElementIpv6Address.ReadNative(element);
                default:
                    throw new InvalidCastException($"Unknown Option Data Type: {element.OptionType}");
            }
        }

        internal abstract DHCP_OPTION_DATA_ELEMENT_Managed ToNative();

        public override string ToString() => $"{Type}: {ValueFormatted}";
    }

    public class DhcpServerOptionElementByte : DhcpServerOptionElement
    {
        public override DhcpServerOptionElementType Type => DhcpServerOptionElementType.Byte;
        public override object Value => RawValue;
        public override string ValueFormatted => BitHelper.ReadHexString(RawValue);

        public byte RawValue { get; }

        internal DhcpServerOptionElementByte(byte value)
        {
            RawValue = value;
        }

        internal static DhcpServerOptionElementByte ReadNative(DHCP_OPTION_DATA_ELEMENT native)
            => new DhcpServerOptionElementByte(native.ByteOption);

        internal override DHCP_OPTION_DATA_ELEMENT_Managed ToNative()
            => new DHCP_OPTION_DATA_ELEMENT_Managed(RawValue);
    }

    public class DhcpServerOptionElementWord : DhcpServerOptionElement
    {
        public override DhcpServerOptionElementType Type => DhcpServerOptionElementType.Word;
        public override object Value => RawValue;
        public override string ValueFormatted => RawValue.ToString("N0");

        public short RawValue { get; }

        internal DhcpServerOptionElementWord(short value)
        {
            RawValue = value;
        }

        internal static DhcpServerOptionElementWord ReadNative(DHCP_OPTION_DATA_ELEMENT native)
            => new DhcpServerOptionElementWord(native.WordOption);

        internal override DHCP_OPTION_DATA_ELEMENT_Managed ToNative()
            => new DHCP_OPTION_DATA_ELEMENT_Managed(RawValue);
    }

    public class DhcpServerOptionElementDWord : DhcpServerOptionElement
    {
        public override DhcpServerOptionElementType Type => DhcpServerOptionElementType.DWord;
        public override object Value => RawValue;
        public override string ValueFormatted => RawValue.ToString("N0");

        public int RawValue { get; }

        internal DhcpServerOptionElementDWord(int value)
        {
            RawValue = value;
        }

        internal static DhcpServerOptionElementDWord ReadNative(DHCP_OPTION_DATA_ELEMENT native)
            => new DhcpServerOptionElementDWord(native.DWordOption);

        internal override DHCP_OPTION_DATA_ELEMENT_Managed ToNative()
            => new DHCP_OPTION_DATA_ELEMENT_Managed(RawValue);
    }

    public class DhcpServerOptionElementDWordDWord : DhcpServerOptionElement
    {
        public override DhcpServerOptionElementType Type => DhcpServerOptionElementType.DWordDWord;
        public override object Value => RawValue;
        public override string ValueFormatted => RawValue.ToString("N0");

        public long RawValue { get; }

        internal DhcpServerOptionElementDWordDWord(long value)
        {
            RawValue = value;
        }

        internal static DhcpServerOptionElementDWordDWord ReadNative(DHCP_OPTION_DATA_ELEMENT native)
            => new DhcpServerOptionElementDWordDWord(native.DWordDWordOption);

        internal override DHCP_OPTION_DATA_ELEMENT_Managed ToNative()
            => new DHCP_OPTION_DATA_ELEMENT_Managed(RawValue);
    }

    public class DhcpServerOptionElementIpAddress : DhcpServerOptionElement
    {
        public override DhcpServerOptionElementType Type => DhcpServerOptionElementType.IpAddress;
        public override object Value => RawValue;
        public override string ValueFormatted => ipAddress;

        private readonly DhcpServerIpAddress ipAddress;

        public uint RawValue => ipAddress.Native;

        internal DhcpServerOptionElementIpAddress(DHCP_IP_ADDRESS value)
        {
            ipAddress = value.AsNetworkToIpAddress();
        }

        internal static DhcpServerOptionElementIpAddress ReadNative(DHCP_OPTION_DATA_ELEMENT native)
            => new DhcpServerOptionElementIpAddress(native.IpAddressOption);

        internal override DHCP_OPTION_DATA_ELEMENT_Managed ToNative()
            => new DHCP_OPTION_DATA_ELEMENT_Managed(new DHCP_IP_ADDRESS(RawValue));
    }

    public class DhcpServerOptionElementString : DhcpServerOptionElement
    {
        public override DhcpServerOptionElementType Type => DhcpServerOptionElementType.StringData;
        public override object Value => RawValue;
        public override string ValueFormatted => RawValue;

        public string RawValue { get; }

        internal DhcpServerOptionElementString(string value)
        {
            RawValue = value;
        }

        internal static DhcpServerOptionElementString ReadNative(DHCP_OPTION_DATA_ELEMENT native)
            => new DhcpServerOptionElementString(Marshal.PtrToStringUni(native.StringDataOption));

        internal override DHCP_OPTION_DATA_ELEMENT_Managed ToNative()
            => new DHCP_OPTION_DATA_ELEMENT_Managed((DHCP_OPTION_DATA_TYPE)Type, RawValue);
    }

    public class DhcpServerOptionElementBinary : DhcpServerOptionElement
    {
        private readonly DhcpServerOptionElementType type;

        public override DhcpServerOptionElementType Type => type;
        public override object Value => RawValue;
        public override string ValueFormatted => (RawValue == null) ? null : BitHelper.ReadHexString(RawValue, ' ');

        public byte[] RawValue { get; }

        internal DhcpServerOptionElementBinary(DhcpServerOptionElementType type, byte[] value)
        {
            this.type = type;
            RawValue = value;
        }

        internal static DhcpServerOptionElementBinary ReadNative(DHCP_OPTION_DATA_ELEMENT native)
            => new DhcpServerOptionElementBinary((DhcpServerOptionElementType)native.OptionType, native.BinaryDataOption.Data);

        internal override DHCP_OPTION_DATA_ELEMENT_Managed ToNative()
            => new DHCP_OPTION_DATA_ELEMENT_Managed((DHCP_OPTION_DATA_TYPE)Type, new DHCP_BINARY_DATA_Managed(RawValue));
    }

    public class DhcpServerOptionElementIpv6Address : DhcpServerOptionElement
    {
        public override DhcpServerOptionElementType Type => DhcpServerOptionElementType.Ipv6Address;
        public override object Value => RawValue;
        public override string ValueFormatted => RawValue;

        public string RawValue { get; }

        internal DhcpServerOptionElementIpv6Address(string value)
        {
            RawValue = value;
        }

        internal static DhcpServerOptionElementIpv6Address ReadNative(DHCP_OPTION_DATA_ELEMENT native)
            => new DhcpServerOptionElementIpv6Address(Marshal.PtrToStringUni(native.Ipv6AddressDataOption));

        internal override DHCP_OPTION_DATA_ELEMENT_Managed ToNative()
            => new DHCP_OPTION_DATA_ELEMENT_Managed((DHCP_OPTION_DATA_TYPE)Type, RawValue);
    }
}
