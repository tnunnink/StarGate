using System;
using System.Collections.Generic;
using ArchestrA.GRAccess;
using StarGate.Core;

namespace StarGate.Galaxy;

/// <summary>
/// Provides extension methods for working with IMxValue instances in the ArchestrA GRAccess framework.
/// </summary>
public static class ValueExtensions
{
    /// <param name="mxValue">The IMxValue instance containing the datetime information.</param>
    extension(IMxValue mxValue)
    {
        /// <summary>
        /// Retrieves the value stored in the IMxValue instance, returning either a scalar value or an array of values
        /// depending on the dimensionality of the IMxValue instance.
        /// </summary>
        /// <returns>
        /// A .NET object representing the value of the IMxValue instance. If the instance represents an array,
        /// returns an object array containing the individual elements. Returns null if the instance is uninitialized,
        /// unassigned, or contains no data.
        /// </returns>
        public object? GetValue()
        {
            if (!mxValue.IsArray())
                return mxValue.GetObject();

            var results = new List<object?>();
            mxValue.GetDimensionSize(out var length);

            for (var i = 1; i <= length; i++)
            {
                var item = new MxValueClass();
                mxValue.GetElement(i, item);
                results.Add(item.GetObject());
            }

            return results.ToArray();
        }

        /// <summary>
        /// Assigns the specified value to the IMxValue instance. Handles both scalar and array types, performing the necessary
        /// type conversions and mapping to ensure compatibility with the data structure of the IMxValue instance.
        /// </summary>
        /// <param name="value">
        /// The value to assign to the IMxValue instance. For arrays, the value must be an enumerable of objects.
        /// For scalar values, the type must match the IMxValue's expected type.
        /// </param>
        public void PutValue(object? value)
        {
            if (value is IEnumerable<object?> enumerable)
            {
                // Clear the value before filling it with the incoming array
                mxValue.Empty();

                // ArchestrA uses one-based notation
                var index = 1;

                foreach (var item in enumerable)
                {
                    var element = new MxValueClass();
                    element.PutObject(item);
                    mxValue.PutElement(index, element);
                    index++;
                }

                return;
            }

            mxValue.PutObject(value);
        }

        /// <summary>
        /// Retrieves the underlying value from the IMxValue instance based on its data type,
        /// converting it to an appropriate .NET object representation.
        /// </summary>
        /// <returns>
        /// An object representing the value of the IMxValue instance. Returns null if the value is uninitialized,
        /// unassigned, or represents "No Data".
        /// </returns>
        private object? GetObject()
        {
            object? value = mxValue.GetDataType() switch
            {
                MxDataType.MxNoData => null,
                MxDataType.MxBoolean => mxValue.GetBoolean(),
                MxDataType.MxInteger => mxValue.GetInteger(),
                MxDataType.MxFloat => mxValue.GetFloat(),
                MxDataType.MxDouble => mxValue.GetDouble(),
                MxDataType.MxString => mxValue.GetString(),
                MxDataType.MxTime => mxValue.GetDateTime(),
                MxDataType.MxElapsedTime => mxValue.GetTimeSpan(),
                MxDataType.MxReferenceType => mxValue.GetMxReference().FullReferenceString,
                MxDataType.MxDataTypeEnum => mxValue.GetMxDataType().ToCoreType(),
                //todo double check but I think GetString works here just fine
                /*MxDataType.MxQualifiedEnum => mxValue.GetCustomEnum(out var name, 0,0 ,0),*/
                MxDataType.MxQualifiedStruct => mxValue.GetBinary(),
                MxDataType.MxInternationalizedString => mxValue.GetInternationalString(0),
                _ => mxValue.GetString()
            };

            return value;
        }

        /// <summary>
        /// Assigns the specified value to the IMxValue instance, performing necessary type conversions
        /// to align with the target data type of the IMxValue.
        /// </summary>
        /// <param name="value">
        /// The value to assign to the IMxValue instance.
        /// The type of the value must be compatible with the target data type of the instance.
        /// </param>
        private void PutObject(object? value)
        {
            if (value is null)
            {
                mxValue.Empty();
                return;
            }

            var mxType = mxValue.GetDataType();

            try
            {
                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                // We will use PutString as the default unless we find a type that doesn't support that.
                switch (mxType)
                {
                    case MxDataType.MxBoolean:
                        mxValue.PutBoolean(Convert.ToBoolean(value));
                        break;
                    case MxDataType.MxInteger:
                        mxValue.PutInteger(Convert.ToInt32(value));
                        break;
                    case MxDataType.MxFloat:
                        mxValue.PutFloat(Convert.ToSingle(value));
                        break;
                    case MxDataType.MxDouble:
                        mxValue.PutDouble(Convert.ToDouble(value));
                        break;
                    case MxDataType.MxString:
                        mxValue.PutString(value.ToString());
                        break;
                    case MxDataType.MxTime:
                        mxValue.PutTime(IMxValue.PutDateTime((DateTime)value));
                        break;
                    case MxDataType.MxElapsedTime:
                        mxValue.PutElapsedTime(IMxValue.PutTimeSpan((TimeSpan)value));
                        break;
                    case MxDataType.MxReferenceType:
                        var reference = mxValue.GetMxReference();
                        reference.FullReferenceString = value.ToString();
                        mxValue.PutMxReference(reference);
                        break;
                    case MxDataType.MxStatusType:
                        //todo would likely want some core type to map to here
                        var status = mxValue.GetMxStatus();
                        status.category = (MxStatusCategory)value;
                        mxValue.PutMxStatus(ref status);
                        break;
                    case MxDataType.MxDataTypeEnum:
                        mxValue.PutMxDataType(((DataType)value).ToMxType());
                        break;
                    case MxDataType.MxSecurityClassificationEnum:
                        mxValue.PutMxSecurityClassification(((Security)value).ToMxType());
                        break;
                    case MxDataType.MxDataQualityType:
                        //todo would likely want some core type to map to here
                        var id = (short)value;
                        mxValue.PutMxDataQuality(ref id);
                        break;
                    case MxDataType.MxQualifiedEnum:
                        mxValue.PutCustomEnum(value.ToString(), 0, 0, 0);
                        break;
                    case MxDataType.MxQualifiedStruct:
                        mxValue.PutCustomStructVB(-1, (byte[])value);
                        break;
                    case MxDataType.MxInternationalizedString:
                        mxValue.PutInternationalString(0, value.ToString());
                        break;
                    case MxDataType.MxBigString:
                        mxValue.PutString(value.ToString());
                        break;
                    default:
                        //todo not sure if this makes sense since we handling all types?
                        mxValue.PutString(value.ToString());
                        break;
                }
            }
            catch (InvalidCastException ex)
            {
                var typeName = value.GetType().Name;
                var message = $"Failed to coerce value '{value}' ({typeName}) to target ArchestrA type {mxType}.";
                throw new InvalidCastException(message, ex);
            }
            catch (FormatException ex)
            {
                var typeName = value.GetType().Name;

                var message = $"Failed to coerce value '{value}' ({typeName}) to target ArchestrA type {mxType}.";
                throw new FormatException(message, ex);
            }
        }

        /// <summary>
        /// Converts the IMxValue instance to a DateTime representation.
        /// </summary>
        /// <returns>A DateTime object representing the underlying value of the IMxValue instance.</returns>
        private DateTime GetDateTime()
        {
            mxValue.GetTime(out var time);
            var high = (ulong)time.HighDateTime;
            var low = (uint)time.LowDateTime;
            var fileTime = (long)((high << 32) + low);
            return DateTime.FromFileTime(fileTime);
        }

        /// <summary>
        /// Converts the IMxValue instance to a TimeSpan representation.
        /// </summary>
        /// <returns>A TimeSpan object representing the elapsed time value of the IMxValue instance.</returns>
        private TimeSpan GetTimeSpan()
        {
            var elapsed = mxValue.GetElapsedTime();
            var high = (ulong)elapsed.HighPart;
            var low = (uint)elapsed.LowPart;
            var time = (long)((high << 32) + low);
            return TimeSpan.FromTicks(time);
        }

        /// <summary>
        /// Retrieves the binary representation of the custom structure stored in the IMxValue instance.
        /// </summary>
        /// <returns>
        /// A byte array containing the binary data of the custom structure. If the IMxValue instance
        /// does not contain valid custom structure data, an empty byte array is returned.
        /// </returns>
        private byte[] GetBinary()
        {
            byte[] data = [];
            mxValue.GetCustomStructVB(out _, ref data);
            return data;
        }

        /// <summary>
        /// Converts a given DateTime value into a VBFILETIME structure and returns the result.
        /// </summary>
        /// <param name="dateTime">The DateTime value to be converted into VBFILETIME format.</param>
        /// <returns>
        /// A VBFILETIME structure containing the HighDateTime and LowDateTime components
        /// that represent the given DateTime value in the FILETIME format.
        /// </returns>
        private static VBFILETIME PutDateTime(DateTime dateTime)
        {
            var fileTime = dateTime.ToFileTime();

            return new VBFILETIME
            {
                HighDateTime = (int)(fileTime >> 32),
                LowDateTime = (int)(fileTime & uint.MaxValue)
            };
        }

        /// <summary>
        /// Converts the specified TimeSpan into a VB_LARGE_INTEGER structure, which contains
        /// the high and low parts of the TimeSpan's tick value.
        /// </summary>
        /// <param name="timeSpan">The TimeSpan to be converted into a VB_LARGE_INTEGER structure.</param>
        /// <returns>
        /// A VB_LARGE_INTEGER structure representing the specified TimeSpan. The high and
        /// low parts correspond to the most significant and least significant 32 bits of the TimeSpan's tick value,
        /// respectively.
        /// </returns>
        private static VB_LARGE_INTEGER PutTimeSpan(TimeSpan timeSpan)
        {
            var ticks = timeSpan.Ticks;

            return new VB_LARGE_INTEGER
            {
                HighPart = (int)(ticks >> 32),
                LowPart = (int)(ticks & uint.MaxValue)
            };
        }

        /// <summary>
        /// Gets a value indicating whether the current <c>IMxValue</c> instance represents an array.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the <c>IMxValue</c> contains multidimensional data (i.e., an array); otherwise, <c>false</c>.
        /// </returns>
        private bool IsArray()
        {
            mxValue.GetDimensionCount(out var dimensions);
            return dimensions == 1;
        }
    }
}