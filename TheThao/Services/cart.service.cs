// Create a new file named cart.service.cs under a suitable folder (e.g., Services)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheThao.Models;
using Microsoft.AspNetCore.Components;

namespace TheThao.Services
{
    public class CartService : ICartService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private GioHang _cart;

        public CartService(HttpClient httpClient, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _baseUrl = navigationManager.BaseUri;
            _cart = new GioHang(); // Initialize an empty cart
        }

        public async Task<GioHang> GetCart()
        {
            if (_cart.Id == 0) // Check if cart is already loaded
            {
                var userId = GetUserIdFromSession();
                if (userId != null)
                {
                    var response = await _httpClient.GetAsync($"{_baseUrl}api/Cart/{userId}");
                    if (response.IsSuccessStatusCode)
                    {
                        _cart = await response.Content.ReadFromJsonAsync<GioHang>();
                    }
                }
            }
            return _cart;
        }

        public async Task SaveCart()
        {
            await _httpClient.PostAsJsonAsync($"{_baseUrl}api/Cart", _cart);
        }

        public async Task UpdateCartItem(int itemId, int quantity)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}api/Cart/UpdateCartItem", new { itemId, quantity });
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error updating cart item on server");
            }
            // Update cart locally after successful update on server
            var cartItem = _cart.ChiTietGioHangs.FirstOrDefault(c => c.Id == itemId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
            }
        }

        private string GetUserIdFromSession()
        {
            // Implement logic to retrieve user ID from session storage (e.g., HttpContext.Session)
            // Replace with your actual implementation
            return "1"; // Replace with actual user ID retrieval logic
        }
    }

    public interface ICartService
    {
        Task<GioHang> GetCart();
        Task SaveCart();
        Task UpdateCartItem(int itemId, int quantity);
    }
}
