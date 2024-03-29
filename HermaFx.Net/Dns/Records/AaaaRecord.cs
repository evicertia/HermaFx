﻿#region License
/**********************************************************************
 * Copyright (c) 2010, j. montgomery                                  *
 * All rights reserved.                                               *
 *                                                                    *
 * Redistribution and use in source and binary forms, with or without *
 * modification, are permitted provided that the following conditions *
 * are met:                                                           *
 *                                                                    *
 * + Redistributions of source code must retain the above copyright   *
 *   notice, this list of conditions and the following disclaimer.    *
 *                                                                    *
 * + Redistributions in binary form must reproduce the above copyright*
 *   notice, this list of conditions and the following disclaimer     *
 *   in the documentation and/or other materials provided with the    *
 *   distribution.                                                    *
 *                                                                    *
 * + Neither the name of j. montgomery's employer nor the names of    *
 *   its contributors may be used to endorse or promote products      *
 *   derived from this software without specific prior written        *
 *   permission.                                                      *
 *                                                                    *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS*
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT  *
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS  *
 * FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE     *
 * COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,*
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES           *
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR *
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) *
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,*
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)      *
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED*
 * OF THE POSSIBILITY OF SUCH DAMAGE.                                 *
 **********************************************************************/
#endregion

using System;
using System.IO;
using System.Text;

namespace HermaFx.Net.Dns.Records
{
	public sealed class AaaaRecord : DnsRecordBase
	{
		private string m_ipAddress;

		internal AaaaRecord(RecordHeader dnsHeader) : base(dnsHeader) { }

		public override void ParseRecord(ref MemoryStream ms)
		{
			// TODO: Test and incorporate BinToHex function below
			m_ipAddress =
				ms.ReadByte().ToString("x2") + ms.ReadByte().ToString("x2") + ":" + ms.ReadByte().ToString("x2") + ms.ReadByte().ToString("x2") + ":" +
				ms.ReadByte().ToString("x2") + ms.ReadByte().ToString("x2") + ":" + ms.ReadByte().ToString("x2") + ms.ReadByte().ToString("x2") + ":" +
				ms.ReadByte().ToString("x2") + ms.ReadByte().ToString("x2") + ":" + ms.ReadByte().ToString("x2") + ms.ReadByte().ToString("x2") + ":" +
				ms.ReadByte().ToString("x2") + ms.ReadByte().ToString("x2") + ":" + ms.ReadByte().ToString("x2") + ms.ReadByte().ToString("x2");
			_answer = "IPv6 Address: " + m_ipAddress;
		}

		// TODO: converted from VB.NET, test to make sure it works properly
		private static string BinToHex(byte[] data)
		{
			if (data != null)
			{
				StringBuilder sb = new System.Text.StringBuilder(1024);
				for (int i = 0; i < data.Length; i++)
				{
					sb.Append(data[i].ToString("X2"));
				}
				return sb.ToString();
			}
			else
			{
				return null;
			}
		}

		// TODO: converted from VB.NET, test to make sure it works properly
		private static byte[] HexToBin(string s)
		{
			int arraySize = s.Length / 2;
			byte[] bytes = new byte[arraySize - 1];
			int counter = 0;

			for (int i = 0; i < s.Length - 1; i = 2)
			{
				string hexValue = s.Substring(i, 2);

				// Tell convert to interpret the string as a 16 bit hex value
				int intValue = Convert.ToInt32(hexValue, 16);
				// Convert the integer to a byte and store it in the array
				bytes[counter] = Convert.ToByte(intValue);
				counter += 1;
			}
			return bytes;
		}
	}
}
