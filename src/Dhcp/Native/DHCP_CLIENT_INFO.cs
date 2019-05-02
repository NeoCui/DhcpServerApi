﻿using System.Runtime.InteropServices;

namespace Dhcp.Native
{
    /// <summary>
    /// The DHCP_CLIENT_INFO structure defines a client information record used by the DHCP server.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DHCP_CLIENT_INFO
    {
        /// <summary>
        /// DHCP_IP_ADDRESS value that contains the assigned IP address of the DHCP client.
        /// </summary>
        public readonly DHCP_IP_ADDRESS ClientIpAddress;
        /// <summary>
        /// DHCP_IP_MASK value that contains the subnet mask value assigned to the DHCP client.
        /// </summary>
        public readonly DHCP_IP_MASK SubnetMask;
        /// <summary>
        /// DHCP_CLIENT_UID structure containing the MAC address of the client's network interface device.
        /// </summary>
        public readonly DHCP_BINARY_DATA ClientHardwareAddress;
        /// <summary>
        /// Unicode string that specifies the network name of the DHCP client. This member is optional.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public readonly string ClientName;
        /// <summary>
        /// Unicode string that contains a comment associated with the DHCP client. This member is optional.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public readonly string ClientComment;
        /// <summary>
        /// DATE_TIME structure that contains the date and time the DHCP client lease will expire, in UTC time.
        /// </summary>
        public readonly DATE_TIME ClientLeaseExpires;
        /// <summary>
        /// DHCP_HOST_INFO structure that contains information on the DHCP server that assigned the IP address to the client. 
        /// </summary>
        public readonly DHCP_HOST_INFO OwnerHost;
    }
}
