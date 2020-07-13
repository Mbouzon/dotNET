using System;
using System.Activities;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BznUiPath.Activities.Properties;
using Newtonsoft.Json;
using UiPath.Shared.Activities;

namespace BznUiPath.Activities
{
    [LocalizedDisplayName(nameof(Resources.BuscaCepActivityDisplayName))]
    [LocalizedDescription(nameof(Resources.BuscaCepActivityDescription))]
    public class BuscaCep : AsyncTaskCodeActivity
    {

        #region Properties
        [LocalizedDisplayName(nameof(Resources.BuscaCepActivityCepDisplayName))]
        [LocalizedDescription(nameof(Resources.BuscaCepActivityCepDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        [RequiredArgument]
        public InArgument<string> Cep { get; set; }

        [LocalizedDisplayName(nameof(Resources.BuscaCepActivityResultDisplayName))]
        [LocalizedDescription(nameof(Resources.BuscaCepActivityResultDescription))]
        [LocalizedCategory(nameof(Resources.Output))]
        public OutArgument<Address> Result { get; set; }

        #endregion

        public BuscaCep()
        {
            Constraints.Add(ActivityConstraints.HasParentType<BuscaCep, ParentScope>(Resources.ValidationMessage));
        }

        #region Protected Methods

        /// <summary>
        /// Validates properties at design-time.
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Cep == null) metadata.AddValidationError(string.Format(Resources.MetadataValidationError, nameof(Cep)));
            base.CacheMetadata(metadata);
        }

        /// <summary>
        /// Runs the main logic of the activity. Has access to the context, 
        /// which holds the values of properties for this activity and those from the parent scope.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            var property = context.DataContext.GetProperties()[ParentScope.ApplicationTag];
            var app = property.GetValue(context.DataContext) as Application;

            string cep = Cep.Get(context);
            if (cep.Length != 8)
                throw new FormatException(string.Format(Resources.CepLenghtValidation));


            var clientWeb = new System.Net.WebClient();
            string json_data = string.Empty;
            Address address = null;

            try
            {
                clientWeb.Encoding = Encoding.UTF8;
                json_data = clientWeb.DownloadString("https://viacep.com.br/ws/" + cep + "/json/");
                //JObject jj = JObject.Parse(json_data);
                address = JsonConvert.DeserializeObject<Address>(json_data);


            }
            catch (Exception) {
                throw new Exception(string.Format(Resources.RunTimeBuscaCepError));
            }

            return ctx => {
                Result.Set(ctx, address);
            };
        }

        #endregion

    }
    public class Address
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string unidade { get; set; }
        public string ibge { get; set; }
        public string gia { get; set; }

    }
}
