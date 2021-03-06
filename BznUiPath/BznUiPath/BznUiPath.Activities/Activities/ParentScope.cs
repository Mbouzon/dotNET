﻿using System;
using System.Activities;
using System.ComponentModel;
using System.Activities.Statements;
using System.Threading;
using System.Threading.Tasks;
using BznUiPath.Activities.Properties;
using BznUiPath;
using UiPath.Shared.Activities;

namespace BznUiPath.Activities
{

    [LocalizedDisplayName(nameof(Resources.ParentScope))]
    [LocalizedDescription(nameof(Resources.ParentScopeDescription))]
    public class ParentScope : AsyncTaskNativeActivity
    {
		#region Properties
    
        private Application Application;

		public const string ApplicationTag = "ParentScopeApplication";

		[Browsable(false)]
        public ActivityAction<Application> Body { get; set; }

        #endregion


        #region Constructors

        public ParentScope()
        {
            Body = new ActivityAction<Application>
            {
                Argument = new DelegateInArgument<Application>(ApplicationTag),
                Handler = new Sequence { DisplayName = "Do" }
            };
        }

        #endregion


        #region Private Methods

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<NativeActivityContext>> ExecuteAsync(NativeActivityContext context, CancellationToken cancellationToken)
        {
            // Schedule the child activities in the scope's body to run and make the client available to them
            if (Body != null)
				context.ScheduleAction<Application > (Body, Application, OnCompleted, OnFaulted);

			// Any actions to perform after the child activities are scheduled go here
			return _ => { };
		}

        private void OnFaulted(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
        {
        }

        private void OnCompleted(NativeActivityContext context, ActivityInstance completedInstance)
        {
        }

        #endregion


        #region Helpers
        
        #endregion
    }
}
