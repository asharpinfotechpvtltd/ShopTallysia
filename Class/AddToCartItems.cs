using EcommerceTelysia.Class;
using EcommerceTelysia.Models;

using Microsoft.AspNetCore.Session;
using Astaberry.Helpers;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EcommerceTelysia.@class
{
    public class AddToCartItems
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AddToCartItems> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddToCartItems(ApplicationDbContext context, ILogger<AddToCartItems> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
      


    
        public List<Item> Usercart { get; set; } = new List<Item>();
        public Cart CartMarketing { get; set; }
        public async Task<IActionResult> OnPostAddToCarts(int Qty, string productid, bool buynow,  string phone, string username)
        {
            try
            {


                // Fetch product details using stored procedure
                              var result = await _context.TblProducts.Where(e => e.skucode == productid).FirstOrDefaultAsync();

                if (result != null)
                {
                  
                    if (buynow)
                    {
                        var childskuItem = new BuyNow
                        {
                           
                            ProductId = productid,
                            ProductName = result.ProductName,
                            Qty = Qty,
                            Price =Convert.ToDouble(result.Price),
                            Image = result.Image
                        };

                        SessionHelper.SetObjectAsJson(_httpContextAccessor.HttpContext.Session, "buynowItem", new List<BuyNow> { childskuItem });
                        return new OkResult();
                    }
                    // Prepare the cart item
                    var item = new Item
                    {
                        Image = result.Image,
                        Price = Convert.ToDouble(result.Price),
                        ProductId = productid,
                        ProductName = result.ProductName,
                        Qty = Qty,
                       
                    
                    };

                   

                    // Check for existing cart item in the database
                    var existingCart = await _context.TblCart.SingleOrDefaultAsync(e =>
                        e.ProductName == result.ProductName &&
                        e.Phone == phone
                       );

                    if (existingCart == null)
                    {
                        CartMarketing = new Cart
                        {
                            ProductId = productid,
                            ProductName = result.ProductName,
                            Image = result.Image,
                           
                            Price = Convert.ToString(result.Price),
                            Qty = Qty,
                          
                            Name = username,
                         
                            Phone = phone,
                          
                        };
                        if (phone != null)
                        {
                            await _context.TblCart.AddAsync(CartMarketing);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        existingCart.Qty += Qty;
                        int sum = existingCart.Qty;
                        Qty = sum;
                        _context.TblCart.Update(existingCart);
                        await _context.SaveChangesAsync();
                    }
                    // Retrieve session cart
                    Usercart = SessionHelper.GetObjectFromJson<List<Item>>(_httpContextAccessor.HttpContext.Session, "cart") ?? new List<Item>();
                    if (Usercart == null)
                    {
                        Usercart = await _context.TblCart
                            .Where(c => c.Phone == phone)
                            .Select(c => new Item
                            {
                                ProductId = c.ProductId,
                                ProductName = c.ProductName,
                                Price = result.Price,
                                Qty = c.Qty,
                                Image = c.Image,
                             
                            })
                            .ToListAsync();
                    }
                    
                    // Handle "Buy Now"


                    // Handle regular cart logic
                    var existingItem = Usercart.FirstOrDefault(i => i.ProductId == productid);
                      

                    if (existingItem != null)
                    {
                        existingItem.Qty += Qty;
                      
                    }
                    else
                    {
                        Usercart.Add(item);
                    }

                    // Update session cart
                    SessionHelper.SetObjectAsJson(_httpContextAccessor.HttpContext.Session, "cart", Usercart);



                    _httpContextAccessor.HttpContext.Session.SetString("productid", productid);
                    _httpContextAccessor.HttpContext.Session.SetInt32("quantity", Qty);
                    
                    return new OkResult();
                }

                return new OkResult();
            }
            catch (Exception ex)
            {
                // Log exception if necessary
                return new BadRequestResult();
            }
        }
      
    }
}
