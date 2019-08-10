using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookstore.Models;
using Helpers;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    public class BooksController : ODataController
    {
        private BookStoreContext _db;

        public BooksController(BookStoreContext context)
        {
            _db = context;
            if (context.Books.Count() == 0)
            {
                foreach (var b in DataSource.GetBooks())
                {
                    context.Books.Add(b);

                }
                context.SaveChanges();
            }
        }


        [EnableQuery]
       // [ODataRoute("Books")]
        //http://localhost:8288/odata/Books?$select=id,ISBN
        public IEnumerable<Book> Get(ODataQueryOptions<Book> options)
        {

            var queryBuilder = new SqlQueryBuilder(options);
            var sql = queryBuilder.ToSql();
            //sql select id from book orderby id
            return _db.Books;
        }


       
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromBody] Book book)
        {
         
            _db.Books.Add(book);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}