namespace Inboxy.EmailReader
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.Exchange.WebServices.Data;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using Task = System.Threading.Tasks.Task;

	public class ExchangeRepository
	{
		private readonly ExchangeService serviceInstance;

		public ExchangeRepository(InboxConfig config)
		{
			this.serviceInstance = new ExchangeService(ExchangeVersion.Exchange2010_SP2)
			{
				UseDefaultCredentials = true
			};

			try
			{
				this.serviceInstance.AutodiscoverUrl(config.Email, SslRedirectionCallback);
			}
			catch (Exception ex)
			{
				this.serviceInstance = null;
				this.ExceptionMessage = ex.Message;
			}
		}

		public string ExceptionMessage { get; }

		public FolderId NewItemsFolderId { get; private set; }

		public FolderId ProcessedItemsFolderId { get; private set; }

		public async Task Initialize(string newItemsFolder, string processedItemsFolder)
		{
			this.NewItemsFolderId = await this.GetFolderId("inboxy-new");
			this.ProcessedItemsFolderId = await this.GetFolderId("inboxy-processed");
		}

		public async Task<PaginatedData<EmailMessage>> Read(Paginator paginator, DateTime? minReceivedDate = null)
		{
			var pageSize = paginator.PageSize ?? 10;

			// Ensure 0-based page index, as it is on the Exchange Server.
			var pageIndex = (paginator.PageIndex ?? 1) - 1;

			var itemView = new ItemView(pageSize, pageIndex * pageSize)
			{
				Traversal = ItemTraversal.Shallow,
				PropertySet = new PropertySet(BasePropertySet.FirstClassProperties, ItemSchema.Subject, ItemSchema.DateTimeReceived)
			};

			itemView.PropertySet.RequestedBodyType = BodyType.Text;

			var filter = new SearchFilter.SearchFilterCollection();

			if (minReceivedDate != null)
			{
				filter.Add(new SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeReceived, minReceivedDate));
			}

			var items = await this.serviceInstance.FindItems(
				this.NewItemsFolderId,
				filter,
				itemView);

			return new PaginatedData<EmailMessage>
			{
				Results = items.Cast<EmailMessage>().ToList(),
				TotalCount = items.TotalCount
			};
		}

		public void SendEmail(string to, string subject, string body)
		{
			var email = new EmailMessage(this.serviceInstance);
			email.ToRecipients.Add(to);
			email.Subject = subject;
			email.Body = new MessageBody(body);
			email.Send();
		}

		private static bool SslRedirectionCallback(string serviceUrl)
		{
			// Return true if the URL is an HTTPS URL.
			return serviceUrl.ToLower().StartsWith("https://");
		}

		private async Task<FolderId> GetFolderId(string folderDisplayName)
		{
			FolderView view = new FolderView(10)
			{
				PropertySet = new PropertySet(BasePropertySet.IdOnly)
				{
					FolderSchema.DisplayName
				}
			};

			SearchFilter searchFilter = new SearchFilter.IsEqualTo(FolderSchema.DisplayName, folderDisplayName);
			view.Traversal = FolderTraversal.Deep;

			FindFoldersResults findFolderResults = await this.serviceInstance.FindFolders(WellKnownFolderName.Inbox, view);

			return findFolderResults.Folders[0].Id;
		}
	}
}