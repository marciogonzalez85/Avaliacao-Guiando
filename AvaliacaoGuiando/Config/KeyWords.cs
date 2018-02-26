using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaliacaoGuiando.Config
{
    public class FornecedorElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base["name"]; }
        }

        [ConfigurationProperty("keywords", IsRequired = true)]
        public string KeyWords
        {
            get { return (string)base["keywords"]; }
        }

        public List<string> KeyWordsList
        {
            get { return KeyWords.Split(';').ToList(); }
        }
    }

    [ConfigurationCollection(typeof(FornecedorElement))]
    public class FornecedorElementCollection : ConfigurationElementCollection
    {
        internal const string PropertyName = "Fornecedor";

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return PropertyName; }
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool IsReadOnly()
        {
            return false;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FornecedorElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FornecedorElement)(element)).Name;
        }

        public FornecedorElement this[int idx]
        {
            get { return (FornecedorElement)BaseGet(idx); }
        }
    }

    public class KeyWordsSection : ConfigurationSection
    {
        [ConfigurationProperty("Fornecedores")]
        private FornecedorElementCollection FornecedoresCollection
        {
            get { return ((FornecedorElementCollection)(base["Fornecedores"])); }
            set { base["Fornecedores"] = value; }
        }

        public List<FornecedorElement> Fornecedores
        {
            get
            {
                if (this.FornecedoresCollection.Count > 0)
                {
                    List<FornecedorElement> lst = new List<FornecedorElement>();
                    var enumerator = this.FornecedoresCollection.GetEnumerator();
                    while (enumerator.MoveNext())
                    {

                        lst.Add((FornecedorElement)enumerator.Current);
                    }

                    return lst;
                }

                return null;
            }
        }
    }
}
