namespace LaTeXTemplate
{
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Uses reflection to get a value at this or a level above.
    /// </summary>
    public class DataChain
    {
        // Note: This class may be sped up using a dictionary to keep a hash of
        // the collection name and a delegate to determining it's value.  
        // Right now it's just something to get by.
        // Also find a better name for this and maybe split it into 3 classes. 
        // It's kinda a chain of responsibility mixed with a binder.

        object _currentObj;

        public DataChain Prev { get; set; }
        public string CurrentValName { get; set; }

        public object CurrentObj 
        {
            get { return _currentObj; } 
            set 
            {
                if (value.GetType() != _objType)
                    throw new ArgumentException(
                        string.Format("Expected an object of type {0} but got {1}", 
                                       value.GetType().ToString(), 
                                       _objType.ToString()));

                _currentObj = value;
            }
        }

        private Type _objType;
        private PropertyInfo[] _props;

        public DataChain(Type type)
        {
            _objType = type;
            _props = _objType.GetProperties();
        }

        public DataChain(object obj)
            : this(obj.GetType())
        {
            CurrentObj = obj;
        }

        public string GetValue(string name)
        {
            var prop = GetProperty(name);
            if (prop != null)
                return (prop.GetValue(CurrentObj, null) ?? "").ToString();

            if (Prev != null)
                return Prev.GetValue(name);

            throw new Exception("Property " + name + " is not found.");
        }

        public System.Collections.IEnumerable GetCollection(string name)
        {
            var colProp = GetProperty(name);
            if (colProp != null)
                return colProp.GetValue(CurrentObj, null) as System.Collections.IEnumerable;

            if (Prev != null)
                return Prev.GetCollection(name);

            throw new Exception("Collection " + name + " is not found.");
        }

        public Type GetCollectionsType(string name)
        {
            try
            {
                return GetProperty(name).PropertyType.GetGenericArguments()[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to determine the type of the collection.", ex);
            }
        }

        private PropertyInfo GetProperty(string name)
        {
            var parts = name.Split(new char[] { '.' });
            int demtersJudgement = parts.GetUpperBound(0);
            if (demtersJudgement > 0 && !parts[demtersJudgement - 1].Equals(CurrentValName))
                return null;

            return _props.Where(p => p.Name == parts[demtersJudgement]).FirstOrDefault();
        }
    }
}