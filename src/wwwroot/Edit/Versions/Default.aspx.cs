using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using N2.Web;
using System.Web.Security;
using N2.Security;

namespace N2.Edit.Versions
{
	[ToolbarPlugin("", "versions", "~/Edit/Versions/Default.aspx?selected={selected}", ToolbarArea.Preview, Targets.Preview,  "~/Edit/Versions/Img/book_previous.gif", 90, ToolTip = "versions", GlobalResourceClassName = "Toolbar")]
	[ControlPanelPendingVersion("There is a newer unpublished version of this item.", 200)]
	public partial class Default : Web.EditPage
	{
		ContentItem publishedItem;

		Persistence.IPersister persister;
		Persistence.IVersionManager versioner;

		protected override void OnInit(EventArgs e)
		{
            hlCancel.NavigateUrl = CancelUrl();

            bool isVersionable = SelectedItem.GetType().GetCustomAttributes(typeof(Persistence.NotVersionableAttribute), true).Length == 0;
            cvVersionable.IsValid = isVersionable;
			
			persister = N2.Context.Persister;
			versioner = N2.Context.Current.Resolve<Persistence.IVersionManager>();

			publishedItem = SelectedItem.VersionOf ?? SelectedItem;

			base.OnInit(e);
		}

		protected void gvHistory_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			ContentItem currentVersion = SelectedItem;
			int id = Convert.ToInt32(e.CommandArgument);
			if (currentVersion.ID == id)
			{
				// do nothing
			}
			else if (e.CommandName == "Publish")
			{
				N2.ContentItem previousVersion = Engine.Persister.Get(id);
				versioner.ReplaceVersion(currentVersion, previousVersion);

				currentVersion.SavedBy = User.Identity.Name;
				persister.Save(currentVersion);

				Refresh(currentVersion, ToolbarArea.Both);
				DataBind();
			}
			else if (e.CommandName == "Delete")
			{
				ContentItem item = Engine.Persister.Get(id);
				persister.Delete(item);
			}
		}

		protected void gvHistory_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{

		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			IList<ContentItem> versions = versioner.GetVersionsOf(publishedItem);

			gvHistory.DataSource = versions;
			gvHistory.DataBind();
		}

		protected override string GetPreviewUrl(ContentItem item)
		{
			if (item.VersionOf == null)
				return item.Url;

			return Url.Parse(item.FindPath(PathData.DefaultAction).RewrittenUrl)
				.AppendQuery("preview", item.ID)
				.AppendQuery("original", item.VersionOf.ID);
		}

		protected bool IsVisible(object dataItem)
		{
			Engine.SecurityManager.IsAuthorized(User, dataItem as ContentItem, Permission.Publish);

			return !IsPublished(dataItem);
		}

		protected bool IsPublished(object dataItem)
		{
			return publishedItem == dataItem;
		}
	}
}
