using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using N2.Integrity;
using N2.Details;
using N2.Templates.Items;
using N2.Web.UI;

namespace N2.Templates.ImageGallery.Items
{
	[Definition("Image Gallery", "ImageGallery", "Displays an image with next/previous thumbnails", "", 220)]
	[RestrictParents(typeof(IStructuralPage))]
	[FieldSet(ImageGallery.GallerySettings, "Gallery Settings", 500, ContainerName = Tabs.Content)]
	public class ImageGallery : AbstractContentPage
	{
		#region GallerySettings
		public const string GallerySettings = "gallerySettings";

		[EditableTextBox("Max Image Width", 200, ContainerName = GallerySettings)]
		public virtual int MaxImageWidth
		{
			get { return (int)(GetDetail("MaxImageWidth") ?? 685); }
			set { SetDetail("MaxImageWidth", value); }
		}

		[EditableTextBox("Max Image Height", 210, ContainerName = GallerySettings)]
		public virtual int MaxImageHeight
		{
			get { return (int)(GetDetail("MaxImageHeight") ?? 685); }
			set { SetDetail("MaxImageHeight", value); }
		}

		[EditableTextBox("Max Thumbnail Width", 220, ContainerName = GallerySettings)]
		public virtual int MaxThumbnailWidth
		{
			get { return (int)(GetDetail("MaxThumbnailWidth") ?? 70); }
			set { SetDetail("MaxThumbnailWidth", value); }
		}

		[EditableTextBox("Max Thumbnail Height", 230, ContainerName = GallerySettings)]
		public virtual int MaxThumbnailHeight
		{
			get { return (int)(GetDetail("MaxThumbnailHeight") ?? 60); }
			set { SetDetail("MaxThumbnailHeight", value); }
		} 
		#endregion

		public override string IconUrl
		{
			get { return "~/ImageGallery/UI/Img/Photos.png"; }
		}

		public override string TemplateUrl
		{
			get { return "~/ImageGallery/UI/Default.aspx"; }
		}
	}
}
