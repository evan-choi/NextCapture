using System;
using System.Text;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NextCapture.Input
{
    public class KeysConverter : TypeConverter, IComparer
    {
        private IDictionary keyNames;
        private List<string> displayOrder;
        private StandardValuesCollection values;

        private const VKeys FirstDigit = VKeys.D0;
        private const VKeys LastDigit = VKeys.D9;
        private const VKeys FirstAscii = VKeys.A;
        private const VKeys LastAscii = VKeys.Z;
        private const VKeys FirstNumpadDigit = VKeys.NumPad0;
        private const VKeys LastNumpadDigit = VKeys.NumPad9;

        private void AddKey(string key, VKeys value)
        {
            keyNames[key] = value;
            displayOrder.Add(key);
        }

        private void Initialize()
        {
            keyNames = new Hashtable(34);
            displayOrder = new List<string>(34);

            AddKey("Return", VKeys.Return);
            AddKey("F12", VKeys.F12);
            AddKey("F11", VKeys.F11);
            AddKey("F10", VKeys.F10);
            AddKey("End", VKeys.End);
            AddKey("Ctrl", VKeys.Control);
            AddKey("F8", VKeys.F8);
            AddKey("F9", VKeys.F9);
            AddKey("Alt", VKeys.Alt);
            AddKey("F4", VKeys.F4);
            AddKey("F5", VKeys.F5);
            AddKey("F6", VKeys.F6);
            AddKey("F7", VKeys.F7);
            AddKey("F1", VKeys.F1);
            AddKey("F2", VKeys.F2);
            AddKey("F3", VKeys.F3);
            AddKey("Next", VKeys.Next);
            AddKey("Insert", VKeys.Insert);
            AddKey("Home", VKeys.Home);
            AddKey("Delete", VKeys.Delete);
            AddKey("Shift", VKeys.Shift);
            AddKey("Prior", VKeys.Prior);
            AddKey("Back", VKeys.Back);

            AddKey("0", VKeys.D0);
            AddKey("1", VKeys.D1);
            AddKey("2", VKeys.D2);
            AddKey("3", VKeys.D3);
            AddKey("4", VKeys.D4);
            AddKey("5", VKeys.D5);
            AddKey("6", VKeys.D6);
            AddKey("7", VKeys.D7);
            AddKey("8", VKeys.D8);
            AddKey("9", VKeys.D9);
        }

        private IDictionary KeyNames
        {
            get
            {
                if (keyNames == null)
                {
                    Initialize();
                }
                return keyNames;
            }
        }

        private List<string> DisplayOrder
        {
            get
            {
                if (displayOrder == null)
                {
                    Initialize();
                }
                return displayOrder;
            }
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string) || sourceType == typeof(Enum[]))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(Enum[]))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public int Compare(object a, object b)
        {
            return String.Compare(ConvertToString(a), ConvertToString(b), false, CultureInfo.InvariantCulture);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {

            if (value is string)
            {

                string text = ((string)value).Trim();

                if (text.Length == 0)
                {
                    return null;
                }
                else
                {
                    string[] tokens = text.Split(new char[] { '+' });
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        tokens[i] = tokens[i].Trim();
                    }

                    VKeys key = 0;
                    bool foundKeyCode = false;

                    for (int i = 0; i < tokens.Length; i++)
                    {
                        object obj = KeyNames[tokens[i]];

                        if (obj == null)
                        {
                            obj = Enum.Parse(typeof(VKeys), tokens[i]);
                        }

                        if (obj != null)
                        {
                            VKeys currentKey = (VKeys)obj;

                            if ((currentKey & VKeys.KeyCode) != 0)
                            {
                                if (foundKeyCode)
                                {
                                    //throw new FormatException(SR.GetString(SR.KeysConverterInvalidKeyCombination));
                                }
                                foundKeyCode = true;
                            }

                            key |= currentKey;
                        }
                        else
                        {
                            //throw new FormatException(SR.GetString(SR.KeysConverterInvalidKeyName, tokens[i]));
                        }
                    }

                    return (object)key;
                }
            }
            else if (value is Enum[])
            {
                long finalValue = 0;
                foreach (Enum e in (Enum[])value)
                {
                    finalValue |= Convert.ToInt64(e, CultureInfo.InvariantCulture);
                }
                return Enum.ToObject(typeof(VKeys), finalValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        [SuppressMessage("Microsoft.Performance", "CA1803:AvoidCostlyCallsWherePossible")]
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }

            if (value is VKeys || value is int)
            {
                bool asString = destinationType == typeof(string);
                bool asEnum = false;
                if (!asString) asEnum = destinationType == typeof(Enum[]);
                if (asString || asEnum)
                {
                    VKeys key = (VKeys)value;
                    bool added = false;
                    ArrayList terms = new ArrayList();
                    VKeys modifiers = (key & VKeys.Modifiers);

                    for (int i = 0; i < DisplayOrder.Count; i++)
                    {
                        string keyString = (string)DisplayOrder[i];
                        VKeys keyValue = (VKeys)keyNames[keyString];
                        if (((int)(keyValue) & (int)modifiers) != 0)
                        {

                            if (asString)
                            {
                                if (added)
                                {
                                    terms.Add("+");
                                }

                                terms.Add((string)keyString);
                            }
                            else
                            {
                                terms.Add(keyValue);
                            }

                            added = true;
                        }
                    }

                    VKeys keyOnly = (key & VKeys.KeyCode);
                    bool foundKey = false;

                    if (added && asString)
                    {
                        terms.Add("+");
                    }

                    for (int i = 0; i < DisplayOrder.Count; i++)
                    {
                        string keyString = (string)DisplayOrder[i];
                        VKeys keyValue = (VKeys)keyNames[keyString];
                        if (keyValue.Equals(keyOnly))
                        {

                            if (asString)
                            {
                                terms.Add((string)keyString);
                            }
                            else
                            {
                                terms.Add(keyValue);
                            }
                            added = true;
                            foundKey = true;
                            break;
                        }
                    }

                    if (!foundKey && Enum.IsDefined(typeof(VKeys), (int)keyOnly))
                    {
                        if (asString)
                        {
                            terms.Add(((Enum)keyOnly).ToString());
                        }
                        else
                        {
                            terms.Add((Enum)keyOnly);
                        }
                    }

                    if (asString)
                    {
                        StringBuilder b = new StringBuilder(32);
                        foreach (string t in terms)
                        {
                            b.Append(t);
                        }
                        return b.ToString();
                    }
                    else
                    {
                        return (Enum[])terms.ToArray(typeof(Enum));
                    }
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (values == null)
            {
                ArrayList list = new ArrayList();

                ICollection keys = KeyNames.Values;

                foreach (object o in keys)
                {
                    list.Add(o);
                }

                list.Sort(this);

                values = new StandardValuesCollection(list.ToArray());
            }
            return values;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
