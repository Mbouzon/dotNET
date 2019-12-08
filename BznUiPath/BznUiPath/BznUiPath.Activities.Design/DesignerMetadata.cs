using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using BznUiPath.Activities.Design.Properties;

namespace BznUiPath.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute =  new CategoryAttribute($"{Resources.Category}");


            builder.AddCustomAttributes(typeof(ParentScope), categoryAttribute);
            builder.AddCustomAttributes(typeof(ParentScope), new DesignerAttribute(typeof(ParentScopeDesigner)));
            builder.AddCustomAttributes(typeof(ParentScope), new HelpKeywordAttribute("https://go.uipath.com"));

            builder.AddCustomAttributes(typeof(BuscaCep), categoryAttribute);
            builder.AddCustomAttributes(typeof(BuscaCep), new DesignerAttribute(typeof(BuscaCep)));
            builder.AddCustomAttributes(typeof(BuscaCep), new HelpKeywordAttribute("https://go.uipath.com"));

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
