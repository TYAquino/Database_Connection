using Lab9Startup.Models;
using Lab9Startup.Services;
using Microsoft.AspNetCore.Components;

namespace Lab9Startup.Components.Pages
{
    public partial class BookDetails : ComponentBase
    {
        private Book book;

        [Parameter]
        public string BookId { get; set; }

        // used to navigate back to the Home page
        [Inject] NavigationManager NavigationManager { get; set; }

        // book database accessor
        BookDbAccessor bookDbAccessor = new BookDbAccessor();

        protected override void OnInitialized()
        {
            // Get the book from the database using the BookId
            book = bookDbAccessor.GetBook(BookId);
        }

        private void EditBook(Book book)
        {
            // Navigate to the EditBook page
            NavigationManager.NavigateTo($"/editbook/{book.BookId}");
        }

        private void DeleteBook(Book book)
        {
            // Delete the book from the database
            bookDbAccessor.DeleteBook(book.BookId);

            NavigationManager.NavigateTo("/");
        }
    }
}
