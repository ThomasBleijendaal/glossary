﻿using System.Threading.Tasks;

namespace Mediator
{
    public interface IOrderService
    {
        Task DeleteOrdersAsync(string userId);
        Task CompleteOrderAsync(string userId, string orderId);
    }
}
