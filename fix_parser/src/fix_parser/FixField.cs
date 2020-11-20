namespace fix_parser
{
    using FixDataDictionary;
    using System;

    public class FixField
    {
        public readonly string FixVersion;
        public readonly int Tag;
        public readonly string Value;
        private string _fieldName;
        private string _valueDescription;
        private bool _isValidFixField;

        public FixField(string fixVersion, int tag, string value)
        {
            this._fieldName = string.Empty;
            this._valueDescription = string.Empty;
            this.FixVersion = fixVersion;
            this.Tag = tag;
            this.Value = value;
        }

        public FixField(string fixVersion, int tag, string value, string fieldName, string valueDescription)
        {
            this._fieldName = string.Empty;
            this._valueDescription = string.Empty;
            this.FixVersion = fixVersion;
            this.Tag = tag;
            this.Value = value;
            this._fieldName = fieldName;
            this._valueDescription = valueDescription;
        }

        public bool Equals(FixField other) => 
            !ReferenceEquals(null, other) ? (!ReferenceEquals(this, other) ? ((other.Tag == this.Tag) && (Equals(other.Value, this.Value) && (Equals(other.FixVersion, this.FixVersion) && (Equals(other.FieldName, this.FieldName) && Equals(other.ValueDescription, this.ValueDescription))))) : true) : false;

        public override bool Equals(object obj) => 
            !ReferenceEquals(null, obj) ? (!ReferenceEquals(this, obj) ? (!(obj.GetType() != typeof(FixField)) ? this.Equals((FixField) obj) : false) : true) : false;

        public override int GetHashCode() => 
            (((((this.FixVersion != null) ? this.FixVersion.GetHashCode() : 0) ^ (this.Tag * 0x18d)) ^ ((this.Value != null) ? this.Value.GetHashCode() : 0)) ^ ((this._fieldName != null) ? this._fieldName.GetHashCode() : 0)) ^ ((this._valueDescription != null) ? this._valueDescription.GetHashCode() : 0);

        public void Initialize()
        {
            if (this.Tag > 0)
            {
                FixDictionaryField fixDictionaryFieldByName = FixDataDictionarySingleton.Instance.GetFixDictionaryFieldByName(this.FixVersion, this.Tag);
                if (fixDictionaryFieldByName != null)
                {
                    this._fieldName = fixDictionaryFieldByName.Name;
                    this._valueDescription = fixDictionaryFieldByName.GetDescription(this.Value);
                    this._isValidFixField = true;
                }
            }
        }

        public string ValueDescription =>
            this._valueDescription;

        public string FieldName =>
            this._fieldName;

        public bool IsValidFixField =>
            this._isValidFixField;
    }
}

