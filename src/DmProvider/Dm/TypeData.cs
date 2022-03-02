using System;
using System.Collections.Generic;

namespace Dm
{
	internal class TypeData
	{
		internal const int ARRAY_TYPE_SHORT = 1;

		internal const int ARRAY_TYPE_INTEGER = 2;

		internal const int ARRAY_TYPE_LONG = 3;

		internal const int ARRAY_TYPE_FLOAT = 4;

		internal const int ARRAY_TYPE_DOUBLE = 5;

		internal object m_dumyData;

		internal int m_offset;

		internal int m_bufLen;

		internal byte[] m_dataBuf;

		internal byte[] m_objBlobDescBuf;

		internal bool m_isFromBlob;

		internal int m_packid = -1;

		internal List<object> m_objRefArr = new List<object>();

		protected TypeData(object val, byte[] dataBuf)
		{
			m_dumyData = null;
			m_dumyData = val;
			m_offset = 0;
			m_bufLen = 0;
			m_dataBuf = dataBuf;
		}

		internal static TypeData[] toStruct(Array objArr, TypeDescriptor desc)
		{
			int structMemSize = desc.GetStructMemSize();
			TypeData[] array = new TypeData[structMemSize];
			for (int i = 0; i < structMemSize; i++)
			{
				if (objArr.GetValue(i) == null || objArr.GetValue(i) is DmStruct || objArr.GetValue(i) is DmArray)
				{
					array[i] = new TypeData(objArr.GetValue(i), null);
					continue;
				}
				switch (desc.m_fieldsObj[i].GetDType())
				{
				case 119:
				case 121:
					array[i] = new TypeData(new DmStruct(toStruct((Array)objArr.GetValue(i), desc.m_fieldsObj[i]), desc.m_fieldsObj[i]), null);
					break;
				case 117:
				case 122:
					array[i] = new TypeData(new DmArray(toArray((Array)objArr.GetValue(i), desc.m_fieldsObj[i]), desc.m_fieldsObj[i]), null);
					break;
				default:
					array[i] = toMemberObj(objArr.GetValue(i), desc.m_fieldsObj[i]);
					break;
				}
			}
			return array;
		}

		internal static TypeData[] toArray(Array objArr, TypeDescriptor desc)
		{
			int length = objArr.Length;
			TypeData[] array = new TypeData[length];
			for (int i = 0; i < length; i++)
			{
				if (objArr.GetValue(i) == null || objArr.GetValue(i) is DmStruct || objArr.GetValue(i) is DmArray)
				{
					array[i] = new TypeData(objArr.GetValue(i), null);
					continue;
				}
				switch (desc.m_arrObj.GetDType())
				{
				case 119:
				case 121:
					array[i] = new TypeData(new DmStruct(toStruct((Array)objArr.GetValue(i), desc.m_arrObj), desc.m_arrObj), null);
					break;
				case 117:
				case 122:
					if (!(objArr.GetValue(i) is Array) && desc.m_arrObj.m_arrObj != null)
					{
						objArr.SetValue(makeupObjToArr(objArr.GetValue(i), desc.m_arrObj), i);
					}
					array[i] = new TypeData(new DmArray(toArray((Array)objArr.GetValue(i), desc.m_arrObj), desc.m_arrObj), null);
					break;
				default:
					array[i] = toMemberObj(objArr.GetValue(i), desc.m_arrObj);
					break;
				}
			}
			return array;
		}

		private static object[] makeupObjToArr(object obj, TypeDescriptor objDesc)
		{
			int dType = objDesc.GetDType();
			bool flag = true;
			int num = 0;
			if (dType == 122)
			{
				flag = false;
				num = objDesc.m_length;
			}
			int dType2 = objDesc.m_arrObj.GetDType();
			if (dType2 == 17 || dType2 == 18 || dType2 == 3)
			{
				string text = "";
				if (obj is int)
				{
					throw new InvalidCastException("makeupObjToArr");
				}
				if (obj is long)
				{
					throw new InvalidCastException("makeupObjToArr");
				}
				if (obj is string)
				{
					text = (string)obj;
					int num2 = (flag ? text.Length : num);
					object[] array = new object[num2];
					byte[] bytes = DmConvertion.GetBytes(text, objDesc.GetServerEncoding());
					for (int i = 0; i < num2; i++)
					{
						array[i] = bytes[i];
					}
					return array;
				}
				throw new InvalidCastException("makeupObjToArr");
			}
			throw new NotSupportedException("makeupObjToArr");
		}

		private static TypeData toMemberObj(object mem, TypeDescriptor desc)
		{
			int scale = desc.GetScale();
			int prec = desc.GetPrec();
			int dType = desc.GetDType();
			if (mem == null)
			{
				return new TypeData(null, null);
			}
			DmParameterInternal dmParameterInternal = new DmParameterInternal(desc.m_conn.m_ConnInst);
			dmParameterInternal.SetCType(dType);
			dmParameterInternal.SetPrecision(prec);
			dmParameterInternal.SetScale(scale);
			dmParameterInternal.SetTypeFlag(1);
			new DmSetValue(desc.GetServerEncoding()).SetObject(dmParameterInternal.GetParamValue()[0], mem, desc.m_conn, null, dType, dmParameterInternal);
			return new TypeData(mem, dmParameterInternal.GetInValue(0));
		}

		private static byte[] TypeDataToBytes(TypeData data, TypeDescriptor desc)
		{
			int dType = desc.GetDType();
			if (data.m_dumyData == null)
			{
				byte[] array = realocBuffer(null, 0, 2);
				DmConvertion.SetByte(array, 0, 0);
				DmConvertion.SetByte(array, 1, 0);
				return array;
			}
			switch (dType)
			{
			case 117:
			{
				byte[] array = arrayToBytes((DmArray)data.m_dumyData, desc);
				byte[] array2 = realocBuffer(null, 0, array.Length + 1 + 1);
				DmConvertion.SetByte(array2, 0, 0);
				int num = 1;
				DmConvertion.SetByte(array2, num, 1);
				num++;
				Array.Copy(array, 0, array2, num, array.Length);
				return array2;
			}
			case 122:
			{
				byte[] array = sarrayToBytes((DmArray)data.m_dumyData, desc);
				byte[] array2 = realocBuffer(null, 0, array.Length + 1 + 1);
				DmConvertion.SetByte(array2, 0, 0);
				int num = 1;
				DmConvertion.SetByte(array2, num, 1);
				num++;
				Array.Copy(array, 0, array2, num, array.Length);
				return array2;
			}
			case 119:
			{
				byte[] array = objToBytes(data.m_dumyData, desc);
				byte[] array2 = realocBuffer(null, 0, array.Length + 1 + 1);
				DmConvertion.SetByte(array2, 0, 0);
				int num = 1;
				DmConvertion.SetByte(array2, num, 1);
				num++;
				Array.Copy(array, 0, array2, num, array.Length);
				return array2;
			}
			case 121:
			{
				byte[] array = recordToBytes((DmStruct)data.m_dumyData, desc);
				byte[] array2 = realocBuffer(null, 0, array.Length + 1 + 1);
				DmConvertion.SetByte(array2, 0, 0);
				int num = 1;
				DmConvertion.SetByte(array2, num, 1);
				num++;
				Array.Copy(array, 0, array2, num, array.Length);
				return array2;
			}
			case 12:
			case 19:
			{
				byte[] array = convertLobToBytes(data.m_dumyData, desc.column.GetCType(), desc.GetServerEncoding());
				byte[] array2 = realocBuffer(null, 0, array.Length + 1 + 1);
				DmConvertion.SetByte(array2, 0, 0);
				int num = 1;
				DmConvertion.SetByte(array2, num, 1);
				num++;
				Array.Copy(array, 0, array2, num, array.Length);
				return array2;
			}
			case 13:
			{
				byte[] array = realocBuffer(null, 0, 2);
				DmConvertion.SetByte(array, 0, 0);
				if (data.m_dataBuf != null && data.m_dataBuf.Length != 0)
				{
					DmConvertion.SetByte(array, 1, data.m_dataBuf[0]);
				}
				else
				{
					DmConvertion.SetByte(array, 1, 0);
				}
				return array;
			}
			default:
			{
				byte[] array = data.m_dataBuf;
				byte[] array2 = realocBuffer(null, 0, array.Length + 1 + 1 + 2);
				DmConvertion.SetByte(array2, 0, 0);
				int num = 1;
				DmConvertion.SetByte(array2, num, 1);
				num++;
				DmConvertion.SetShort(array2, num, (short)array.Length);
				num += 2;
				Array.Copy(array, 0, array2, num, array.Length);
				return array2;
			}
			}
		}

		private static byte[] convertLobToBytes(object value, int dtype, string serverEncoding)
		{
			switch (dtype)
			{
			case 12:
			{
				DmBlob obj2 = (DmBlob)value;
				int num = obj2.Length();
				byte[] bytes2 = obj2.GetBytes(0L, num);
				byte[] array = new byte[num + 4];
				DmConvertion.SetInt(array, 0, num);
				Array.Copy(bytes2, 0, array, 4, num);
				return array;
			}
			case 19:
			{
				DmClob obj = (DmClob)value;
				int num = obj.Length();
				byte[] bytes = DmConvertion.GetBytes(obj.GetSubString(0L, num), serverEncoding);
				byte[] array = new byte[bytes.Length + 4];
				DmConvertion.SetInt(array, 0, num);
				Array.Copy(bytes, 0, array, 4, num);
				return array;
			}
			default:
				throw new InvalidCastException();
			}
		}

		public static byte[] sarrayToBytes(DmArray data, TypeDescriptor desc)
		{
			int num = data.m_arrData.Length;
			byte[][] array = new byte[num][];
			if (desc.GetObjId() == 4)
			{
				return ctlnToBytes(data, desc);
			}
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				array[i] = TypeDataToBytes(data.m_arrData[i], desc.m_arrObj);
				num2 += array[i].Length;
			}
			num2 += 8;
			byte[] array2 = realocBuffer(null, 0, num2);
			int num3 = 0;
			DmConvertion.SetInt(array2, num3, num2);
			num3 += 4;
			DmConvertion.SetInt(array2, num3, data.m_arrDesc.GetLength());
			num3 += 4;
			for (int j = 0; j < num; j++)
			{
				Array.Copy(array[j], 0, array2, num3, array[j].Length);
				num3 += array[j].Length;
			}
			return array2;
		}

		public static byte[] ctlnToBytes(DmArray data, TypeDescriptor desc)
		{
			byte[][] array = new byte[data.m_arrData.Length][];
			int num = 5;
			num += 8;
			for (int i = 0; i < data.m_arrData.Length; i++)
			{
				array[i] = TypeDataToBytes(data.m_arrData[i], desc.m_arrObj);
				num += array[i].Length;
			}
			byte[] array2 = realocBuffer(null, 0, num);
			int num2 = 0;
			DmConvertion.SetByte(array2, num2, 0);
			num2++;
			num2 += 4;
			DmConvertion.SetShort(array2, num2, (short)desc.GetCltnType());
			num2 += 2;
			DmConvertion.SetShort(array2, num2, (short)desc.m_arrObj.GetDType());
			num2 += 2;
			DmConvertion.SetInt(array2, num2, data.m_arrData.Length);
			num2 += 4;
			for (int j = 0; j < data.m_arrData.Length; j++)
			{
				Array.Copy(array[j], 0, array2, num2, array[j].Length);
				num2 += array[j].Length;
			}
			DmConvertion.SetInt(array2, 1, num2);
			return array2;
		}

		public static byte[] arrayToBytes(DmArray data, TypeDescriptor desc)
		{
			byte[][] array = new byte[data.m_arrData.Length][];
			if (desc.GetObjId() == 4)
			{
				return ctlnToBytes(data, desc);
			}
			int num = 0;
			for (int i = 0; i < data.m_arrData.Length; i++)
			{
				array[i] = TypeDataToBytes(data.m_arrData[i], desc.m_arrObj);
				num += array[i].Length;
			}
			num += 20;
			int num2 = data.m_objCount + data.m_strCount;
			if (num2 > 0)
			{
				num += 2 * num2;
			}
			byte[] array2 = realocBuffer(null, 0, num);
			DmConvertion.SetInt(array2, 0, num);
			int num3 = 4;
			DmConvertion.SetInt(array2, num3, data.m_arrData.Length);
			num3 += 4;
			DmConvertion.SetInt(array2, num3, 0);
			num3 += 4;
			DmConvertion.SetInt(array2, num3, data.m_objCount);
			num3 += 4;
			DmConvertion.SetInt(array2, num3, data.m_strCount);
			num3 += 4;
			for (int j = 0; j < num2; j++)
			{
				DmConvertion.SetInt(array2, num3, data.m_objStrOffs[j]);
				num3 += 4;
			}
			for (int k = 0; k < data.m_arrData.Length; k++)
			{
				Array.Copy(array[k], 0, array2, num3, array[k].Length);
				num3 += array[k].Length;
			}
			return array2;
		}

		public static byte[] objToBytes(object data, TypeDescriptor desc)
		{
			if (data is DmArray)
			{
				return arrayToBytes((DmArray)data, desc);
			}
			return structToBytes((DmStruct)data, desc);
		}

		public static byte[] structToBytes(DmStruct data, TypeDescriptor desc)
		{
			int structMemSize = desc.GetStructMemSize();
			byte[][] array = new byte[structMemSize][];
			int num = 0;
			for (int i = 0; i < structMemSize; i++)
			{
				array[i] = TypeDataToBytes(data.m_attribs[i], desc.m_fieldsObj[i]);
				num += array[i].Length;
			}
			num += 5;
			byte[] array2 = realocBuffer(null, 0, num);
			int num2 = 0;
			DmConvertion.SetByte(array2, num2, 0);
			num2++;
			DmConvertion.SetInt(array2, num2, num);
			num2 += 4;
			for (int j = 0; j < structMemSize; j++)
			{
				Array.Copy(array[j], 0, array2, num2, array[j].Length);
				num2 += array[j].Length;
			}
			return array2;
		}

		public static byte[] recordToBytes(DmStruct data, TypeDescriptor desc)
		{
			int structMemSize = desc.GetStructMemSize();
			byte[][] array = new byte[structMemSize][];
			int num = 0;
			for (int i = 0; i < structMemSize; i++)
			{
				array[i] = TypeDataToBytes(data.m_attribs[i], desc.m_fieldsObj[i]);
				num += array[i].Length;
			}
			num += 4;
			byte[] array2 = realocBuffer(null, 0, num);
			DmConvertion.SetInt(array2, 0, num);
			int num2 = 4;
			for (int j = 0; j < desc.GetStructMemSize(); j++)
			{
				Array.Copy(array[j], 0, array2, num2, array[j].Length);
				num2 += array[j].Length;
			}
			return array2;
		}

		private static TypeData bytesToBlob(byte[] val, TypeData outVal, TypeDescriptor desc)
		{
			int offset = outVal.m_offset;
			int @int = DmConvertion.GetInt(val, offset);
			offset += 4;
			DmConvertion.GetBytes(val, offset, @int);
			offset = (outVal.m_offset = offset + @int);
			throw new NotSupportedException("bytesToBlob");
		}

		private static TypeData bytesToClob(byte[] val, TypeData outVal, TypeDescriptor desc, string serverEncoding)
		{
			int offset = outVal.m_offset;
			int @int = DmConvertion.GetInt(val, offset);
			offset += 4;
			DmConvertion.GetBytes(val, offset, @int);
			offset = (outVal.m_offset = offset + @int);
			throw new NotSupportedException("bytesToClob");
		}

		private static TypeData bytesToTypeData(byte[] val, TypeData outVal, TypeDescriptor desc)
		{
			int offset = outVal.m_offset;
			offset++;
			byte @byte = DmConvertion.GetByte(val, offset);
			offset = (outVal.m_offset = offset + 1);
			if (desc.GetDType() == 13)
			{
				bool flag = false;
				if (@byte != 0)
				{
					flag = true;
				}
				return new TypeData(flag, DmConvertion.GetBytes(val, offset - 1, 1));
			}
			byte[] dataBuf = null;
			switch (desc.GetDType())
			{
			case 119:
				if (((uint)@byte & (true ? 1u : 0u)) != 0)
				{
					object val2 = bytesToObj(val, outVal, desc);
					if (outVal.m_offset > offset)
					{
						dataBuf = DmConvertion.GetBytes(val, offset, outVal.m_offset - offset);
					}
					return new TypeData(val2, dataBuf);
				}
				return new TypeData(null, null);
			case 117:
				if (((uint)@byte & (true ? 1u : 0u)) != 0)
				{
					DmArray val3 = bytesToArray(val, outVal, desc);
					if (outVal.m_offset > offset)
					{
						dataBuf = DmConvertion.GetBytes(val, offset, outVal.m_offset - offset);
					}
					return new TypeData(val3, dataBuf);
				}
				return new TypeData(null, null);
			case 121:
				if (((uint)@byte & (true ? 1u : 0u)) != 0)
				{
					DmStruct val5 = bytesToRecord(val, outVal, desc);
					if (outVal.m_offset > offset)
					{
						dataBuf = DmConvertion.GetBytes(val, offset, outVal.m_offset - offset);
					}
					return new TypeData(val5, dataBuf);
				}
				return new TypeData(null, null);
			case 122:
				if (((uint)@byte & (true ? 1u : 0u)) != 0)
				{
					DmArray val4 = bytesToSArray(val, outVal, desc);
					if (outVal.m_offset > offset)
					{
						dataBuf = DmConvertion.GetBytes(val, offset, outVal.m_offset - offset);
					}
					return new TypeData(val4, dataBuf);
				}
				return new TypeData(null, null);
			case 12:
				if (((uint)@byte & (true ? 1u : 0u)) != 0)
				{
					return bytesToBlob(val, outVal, desc);
				}
				return new TypeData(null, null);
			case 19:
				if (((uint)@byte & (true ? 1u : 0u)) != 0)
				{
					return bytesToClob(val, outVal, desc, desc.GetServerEncoding());
				}
				return new TypeData(null, null);
			default:
				if (((uint)@byte & (true ? 1u : 0u)) != 0)
				{
					return convertBytes2BaseData(val, outVal, desc);
				}
				return new TypeData(null, null);
			}
		}

		private static bool checkObjExist(byte[] val, TypeData outVal)
		{
			int offset = outVal.m_offset;
			byte @byte = DmConvertion.GetByte(val, offset);
			offset = (outVal.m_offset = offset + 1);
			if (@byte == 1)
			{
				return true;
			}
			outVal.m_offset += 4;
			return false;
		}

		private static DmStruct findObjByPackId(byte[] val, TypeData outVal)
		{
			int offset = outVal.m_offset;
			int @int = DmConvertion.GetInt(val, offset);
			offset = (outVal.m_offset = offset + 4);
			if (@int < 0 || @int > outVal.m_packid)
			{
				throw new InvalidCastException("findObjByPackId");
			}
			return (DmStruct)outVal.m_objRefArr[@int];
		}

		private static void addObjToRefArr(TypeData outVal, object objToAdd)
		{
			outVal.m_objRefArr.Add(objToAdd);
			outVal.m_packid++;
		}

		private static bool checkObjClnt(TypeDescriptor desc)
		{
			return desc.m_objId == 4;
		}

		private static DmStruct bytesToObj_EXACT(byte[] val, TypeData outVal, TypeDescriptor desc)
		{
			DmStruct dmStruct = new DmStruct(null, desc);
			int offset = outVal.m_offset;
			int structMemSize = desc.GetStructMemSize();
			outVal.m_offset = offset;
			dmStruct.m_attribs = new TypeData[structMemSize];
			for (int i = 0; i < structMemSize; i++)
			{
				TypeDescriptor desc2 = desc.m_fieldsObj[i];
				dmStruct.m_attribs[i] = bytesToTypeData(val, outVal, desc2);
			}
			dmStruct.m_dataBuf = DmConvertion.GetBytes(val, offset, outVal.m_offset - offset);
			return dmStruct;
		}

		private static DmArray bytesToNestTab(byte[] val, TypeData outVal, TypeDescriptor desc)
		{
			int offset = outVal.m_offset;
			offset += 2;
			int @int = DmConvertion.GetInt(val, offset);
			offset = (outVal.m_offset = offset + 4);
			DmArray dmArray = new DmArray(null, desc);
			dmArray.m_itemCount = @int;
			dmArray.m_arrData = new TypeData[@int];
			for (int i = 0; i < @int; i++)
			{
				dmArray.m_arrData[i] = bytesToTypeData(val, outVal, desc.m_arrObj);
			}
			dmArray.m_dataBuf = DmConvertion.GetBytes(val, offset, outVal.m_offset - offset);
			return dmArray;
		}

		private static DmArray bytesToClnt(byte[] val, TypeData outVal, TypeDescriptor desc)
		{
			DmArray result = null;
			int offset = outVal.m_offset;
			int @short = DmConvertion.GetShort(val, offset);
			offset = (outVal.m_offset = offset + 2);
			switch (@short)
			{
			case 3:
				throw new NotSupportedException("bytesToClnt");
			case 1:
			case 2:
				result = bytesToNestTab(val, outVal, desc);
				break;
			}
			return result;
		}

		public static object bytesToObj(byte[] val, TypeData outVal, TypeDescriptor desc)
		{
			object objToAdd = null;
			if (outVal == null)
			{
				outVal = new TypeData(null, null);
			}
			if (checkObjExist(val, outVal))
			{
				return findObjByPackId(val, outVal);
			}
			addObjToRefArr(outVal, objToAdd);
			if (checkObjClnt(desc))
			{
				return bytesToClnt(val, outVal, desc);
			}
			return bytesToObj_EXACT(val, outVal, desc);
		}

		public static DmArray bytesToArray(byte[] val, TypeData outVal, TypeDescriptor desc)
		{
			DmArray dmArray = new DmArray(null, desc);
			if (outVal == null)
			{
				outVal = new TypeData(null, null);
			}
			int offset = outVal.m_offset;
			dmArray.m_bufLen = DmConvertion.GetInt(val, offset);
			offset += 4;
			dmArray.m_itemCount = DmConvertion.GetInt(val, offset);
			offset += 4;
			dmArray.m_itemSize = DmConvertion.GetInt(val, offset);
			offset += 4;
			dmArray.m_objCount = DmConvertion.GetInt(val, offset);
			offset += 4;
			dmArray.m_strCount = DmConvertion.GetInt(val, offset);
			offset += 4;
			int num = dmArray.m_objCount + dmArray.m_strCount;
			dmArray.m_objStrOffs = new int[num];
			for (int i = 0; i < num; i++)
			{
				dmArray.m_objStrOffs[i] = DmConvertion.GetInt(val, offset);
				offset += 4;
			}
			outVal.m_offset = offset;
			dmArray.m_arrData = new TypeData[dmArray.m_itemCount];
			for (int j = 0; j < dmArray.m_itemCount; j++)
			{
				dmArray.m_arrData[j] = bytesToTypeData(val, outVal, desc.m_arrObj);
			}
			dmArray.m_dataBuf = DmConvertion.GetBytes(val, offset, outVal.m_offset - offset);
			return dmArray;
		}

		public static DmArray bytesToSArray(byte[] val, TypeData outVal, TypeDescriptor desc)
		{
			if (outVal == null)
			{
				outVal = new TypeData(null, null);
			}
			int offset = outVal.m_offset;
			DmArray dmArray = new DmArray(null, desc);
			dmArray.m_bufLen = DmConvertion.GetInt(val, offset);
			offset += 4;
			dmArray.m_itemCount = DmConvertion.GetInt(val, offset);
			offset = (outVal.m_offset = offset + 4);
			dmArray.m_arrData = new TypeData[dmArray.m_itemCount];
			for (int i = 0; i < dmArray.m_itemCount; i++)
			{
				dmArray.m_arrData[i] = bytesToTypeData(val, outVal, desc.m_arrObj);
			}
			dmArray.m_dataBuf = DmConvertion.GetBytes(val, offset, outVal.m_offset - offset);
			return dmArray;
		}

		public static DmStruct bytesToRecord(byte[] val, TypeData outVal, TypeDescriptor desc)
		{
			if (outVal == null)
			{
				outVal = new TypeData(null, null);
			}
			int offset = outVal.m_offset;
			DmStruct dmStruct = new DmStruct(null, desc);
			dmStruct.m_bufLen = DmConvertion.GetInt(val, offset);
			offset = (outVal.m_offset = offset + 4);
			dmStruct.m_attribs = new TypeData[desc.GetStructMemSize()];
			for (int i = 0; i < desc.GetStructMemSize(); i++)
			{
				dmStruct.m_attribs[i] = bytesToTypeData(val, outVal, desc.m_fieldsObj[i]);
			}
			dmStruct.m_dataBuf = DmConvertion.GetBytes(val, offset, outVal.m_offset - offset);
			return dmStruct;
		}

		private static void objBlob_GetChkBuf(byte[] buf, TypeData typeData)
		{
			int num = 4;
			int @int = DmConvertion.GetInt(buf, num);
			num += 4;
			typeData.m_objBlobDescBuf = DmConvertion.GetBytes(buf, num, @int);
			num += @int;
			typeData.m_isFromBlob = true;
			typeData.m_offset = num;
		}

		public static object objBlobToObj(DmBlob lob, TypeDescriptor desc)
		{
			TypeData typeData = new TypeData(null, null);
			int length = lob.Length();
			byte[] bytes = lob.GetBytes(0L, length);
			objBlob_GetChkBuf(bytes, typeData);
			return bytesToObj(bytes, typeData, desc);
		}

		public static byte[] objBlobToBytes(byte[] lobBuf, TypeDescriptor desc)
		{
			int num = lobBuf.Length;
			int num2 = 0;
			int @int = DmConvertion.GetInt(lobBuf, num2);
			num2 += 4;
			if (78111999 != @int)
			{
				throw new NotSupportedException("objBlobToBytes");
			}
			int int2 = DmConvertion.GetInt(lobBuf, num2);
			num2 += 4;
			byte[] bytes = DmConvertion.GetBytes(lobBuf, num2, int2);
			if (bytes.Length != desc.GetClassDescChkInfo().Length)
			{
				throw new NotSupportedException("objBlobToBytes");
			}
			for (int i = 0; i < bytes.Length; i++)
			{
				if (bytes[i] != desc.GetClassDescChkInfo()[i])
				{
					throw new NotSupportedException("objBlobToBytes");
				}
			}
			num2 += int2;
			byte[] array = new byte[num - num2];
			Array.Copy(lobBuf, num2, array, 0, array.Length);
			return array;
		}

		private static byte[] realocBuffer(byte[] oldBuf, int offset, int needLen)
		{
			if (oldBuf == null)
			{
				return new byte[needLen];
			}
			byte[] array;
			if (needLen + offset > oldBuf.Length)
			{
				array = new byte[oldBuf.Length + needLen];
				Array.Copy(oldBuf, 0, array, 0, offset);
			}
			else
			{
				array = oldBuf;
			}
			return array;
		}

		private static TypeData convertBytes2BaseData(byte[] val, TypeData outVal, TypeDescriptor desc)
		{
			int offset = outVal.m_offset;
			bool flag = false;
			int num = DmConvertion.GetShort(val, offset);
			offset += 2;
			if (num == -2)
			{
				num = 0;
				flag = true;
			}
			if (-1 == num)
			{
				num = DmConvertion.GetInt(val, offset);
				offset += 4;
			}
			if (flag)
			{
				outVal.m_offset = offset;
				return new TypeData(null, null);
			}
			byte[] bytes = DmConvertion.GetBytes(val, offset, num);
			offset = (outVal.m_offset = offset + num);
			return new TypeData(new DmGetValue(desc.m_conn.ConnProperty.ServerEncoding, null, desc.m_conn.ConnProperty.NewLobFlag).GetObject(1, bytes, desc.column.GetCType(), desc.column.GetPrecision(), desc.column.GetScale()), bytes);
		}

		public object toJavaArray(DmArray arr, int len, int dType)
		{
			return toJavaArray(arr, 1L, len, dType);
		}

		public object toJavaArray(DmArray arr, long index, int len, int dType)
		{
			throw new NotSupportedException("toJavaArray");
		}

		public object toNumericArray(DmArray arr, long index, int len, int flag)
		{
			throw new NotSupportedException("toNumericArray");
		}

		public object[] toJavaArray(DmStruct stru)
		{
			throw new NotSupportedException("toJavaArray");
		}

		public static byte[] toBytes(TypeData x, TypeDescriptor typeDesc)
		{
			byte[] classDescChkInfo = typeDesc.GetClassDescChkInfo();
			byte[] array = null;
			switch (typeDesc.GetDType())
			{
			case 117:
				array = arrayToBytes((DmArray)x, typeDesc);
				break;
			case 122:
				array = sarrayToBytes((DmArray)x, typeDesc);
				break;
			case 121:
				array = recordToBytes((DmStruct)x, typeDesc);
				break;
			case 119:
				array = objToBytes(x, typeDesc);
				break;
			}
			byte[] array2 = new byte[8 + classDescChkInfo.Length + array.Length];
			DmConvertion.SetInt(array2, 0, 78111999);
			DmConvertion.SetInt(array2, 4, classDescChkInfo.Length);
			Array.Copy(classDescChkInfo, 0, array2, 8, classDescChkInfo.Length);
			Array.Copy(array, 0, array2, 8 + classDescChkInfo.Length, array.Length);
			return array2;
		}
	}
}
