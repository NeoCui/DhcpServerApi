﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Dhcp.Native
{
    /// <summary>
    /// The DHCP_SUBNET_ELEMENT_INFO_ARRAY_V5 structure defines an array of subnet element data. Element data in the V5 structure is BOOTP specific.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct DHCP_SUBNET_ELEMENT_INFO_ARRAY_V5
    {
        /// <summary>
        /// Specifies the number of elements in Elements.
        /// </summary>
        public readonly int NumElements;

        /// <summary>
        /// Pointer to a list of DHCP_SUBNET_ELEMENT_DATA_V5 structures that contain the data for the corresponding subnet elements.
        /// </summary>
        private readonly IntPtr ElementsPointer;

        /// <summary>
        /// Pointer to a list of DHCP_SUBNET_ELEMENT_DATA_V5 structures that contain the data for the corresponding subnet elements.
        /// </summary>
        public IEnumerable<DHCP_SUBNET_ELEMENT_DATA_V5> Elements
        {
            get
            {
                var instanceIter = ElementsPointer;
                var instanceSize = Marshal.SizeOf(typeof(DHCP_SUBNET_ELEMENT_DATA_V5));
                for (var i = 0; i < NumElements; i++)
                {
                    yield return instanceIter.MarshalToStructure<DHCP_SUBNET_ELEMENT_DATA_V5>();
                    instanceIter += instanceSize;
                }
            }
        }
    }
}
