﻿using System;
using System.Runtime.InteropServices;

namespace Dhcp.Native
{
    /// <summary>
    /// The DHCP_CLIENT_INFO_VQ structure defines information about the DHCPv4 client.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DHCP_CLIENT_INFO_VQ : IDisposable
    {
        /// <summary>
        /// DHCP_IP_ADDRESS type value that contains the DHCPv4 client's IPv4 address. 
        /// </summary>
        public readonly DHCP_IP_ADDRESS ClientIpAddress;
        /// <summary>
        /// DHCP IP_MASK type value that contains the DHCPv4 client's IPv4 subnet mask address.
        /// </summary>
        public readonly DHCP_IP_MASK SubnetMask;
        /// <summary>
        /// GUID value that contains the hardware address (MAC address) of the DHCPv4 client.
        /// </summary>
        public readonly DHCP_BINARY_DATA ClientHardwareAddress;
        /// <summary>
        /// Pointer to a null-terminated Unicode string that represents the DHCPv4 client's machine name.
        /// </summary>
        private IntPtr ClientNamePointer;
        /// <summary>
        /// Pointer to a null-terminated Unicode string that represents the description given to the DHCPv4 client.
        /// </summary>
        private IntPtr ClientCommentPointer;
        /// <summary>
        /// DATE_TIME structure that contains the lease expiry time for the DHCPv4 client. This is UTC time represented in the FILETIME format.
        /// </summary>
        public readonly DATE_TIME ClientLeaseExpires;
        /// <summary>
        /// DHCP_HOST_INFO structure that contains information about the host machine (DHCPv4 server machine) that has provided a lease to the DHCPv4 client.
        /// </summary>
        public readonly DHCP_HOST_INFO OwnerHost;
        /// <summary>
        /// Possible types of the DHCPv4 client.
        /// </summary>
        public readonly ClientTypes bClientType;
        /// <summary>
        /// Possible states of the IPv4 address given to the DHCPv4 client.
        /// </summary>
        public readonly byte AddressState;
        /// <summary>
        /// QuarantineStatus enumeration that specifies possible health status values for the DHCPv4 client, as validated at the NAP server.
        /// </summary>
        public readonly QuarantineStatus Status;
        /// <summary>
        /// This is of type DATE_TIME, containing the end time of the probation if the DHCPv4 client is on probation. For this time period, the DHCPv4 client has full access to the network.
        /// </summary>
        public readonly DATE_TIME ProbationEnds;
        /// <summary>
        /// If TRUE, the DHCPv4 client is quarantine-enabled; if FALSE, it is not.
        /// </summary>
        public readonly bool QuarantineCapable;

        /// <summary>
        /// Pointer to a null-terminated Unicode string that represents the DHCPv4 client's machine name.
        /// </summary>
        public string ClientName => Marshal.PtrToStringUni(ClientNamePointer);

        /// <summary>
        /// Pointer to a null-terminated Unicode string that represents the description given to the DHCPv4 client.
        /// </summary>
        public string ClientComment => Marshal.PtrToStringUni(ClientCommentPointer);

        public void Dispose()
        {
            ClientHardwareAddress.Dispose();
            Api.FreePointer(ref ClientNamePointer);
            Api.FreePointer(ref ClientCommentPointer);
            OwnerHost.Dispose();
        }
    }
}
