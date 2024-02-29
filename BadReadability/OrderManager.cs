namespace BadReadability
{
    internal class OrderManager
    {
        public async Task<OrderDeliveryInfo> CreateExternalOrder(OrderInfo order)
        {
            if (order.IsDraft)
                return await CreateExternalOrderFromDraft(order);

            try
            {

                var delivery = await GetOrderDelivery(order);

                var merchant = await GetMerchantById(order.Sender.MerchantId);

                FilterOutDeliveryOptionsAccordingToMerchantSettings(delivery, merchant);
                
                var merchantOrder = GetMerchantOrderFromDelivery(delivery);

                var orderMobileAppLink = await CreateMobileAppOrderLinkForCourier(delivery, merchant);

                return await SaveAllOrderInfoAndScheduleTaxiServicesCalls(merchantOrder, order, orderMobileAppLink);            
               
            }
            catch (Exception e)
            {
                LogWriter.LogWrite($"taxi order error {e}");
                throw;
            }
        }

        private async Task<DeliveryRequest> GetOrderDelivery(OrderInfo order)
        {
            if (order.DeliveryRequestId == null) throw new Exception("Delivery request is empty");

            var delivery = await GetDeliveryRequest(order.DeliveryRequestId.Value);
            if (delivery == null) throw new Exception("Delivery service not found");

            if (CheckOrderForDeliveryAlreadyExists(order.DeliveryRequestId.Value)) return null;

            return delivery;
        }

        private Task SaveAllOrderInfoAndScheduleTaxiServicesCalls(GDEContext context, MerchantOrder merchantOrder, Order order)
        {
            context.Database.BeginTransaction();

            var merchantOrderDb = SaveMerchantOrderToDb(context, merchantOrder, order.DeliveryRequestId);

            await CreateCallsToTaxiServices(merchantOrder, order, orderMobileAppLink);

            var orderDb = SaveOrderToDb(context, orderId, merchantOrderDb.Id, delivery, null, null, order.Orders.FirstOrDefault()?.Time, order.IsFastDelivery);
            LogWriter.LogWrite("order saved to db");

            await ProcessClientOrders(merchantOrderDb);

            orderDb.IsDraft = order.IsDraft;
            orderDb.LinkToMobile = orderMobileAppLink;

            context.Database.CommitTransaction();
            context.SaveChanges();
            return new OrderDeliveryInfo
            {
                Id = merchantOrderDb.ClientOrders.First().Id,
                OrderId = orderDb.Id,
                Status = MainOrderStatus.New.ToString()
            };
        }

        private Task ProcessClientOrders(object merchantOrderDb)
        {
            foreach (var clientOrder in merchantOrderDb.ClientOrders)
            {
                var clientDb = await GetOrCreateClient(clientOrder, context);
                clientOrder.LinkToMobile = await CreateMobileAppOrderLinkForClient(clientDb.User, clientOrder.Id);
            }
        }

        private async Task<DeliveryRequest> GetDeliveryRequest(Guid deliveryRequestId)
        {
            using (var context = new GDEContext())
            {
                return await context.Set<DeliveryRequest>().Include(d => d.Options)
                    .FirstOrDefaultAsync(d => d.Id == deliveryRequestId);
            }
        }

        private Task CreateCallsToTaxiServices(MerchantOrder merchantOrder, OrderInfo order, object orderMobileAppLink)
        {
            var time = order.Orders.FirstOrDefault()?.Time - 10;
            if (time > 0)
            {
                Observable
                    .Timer(TimeSpan.FromMinutes((double)time)).Subscribe(async x =>
                    {
                        await taxiHelper.CreateTaxiOrders(delivery, merchantOrder, link, orderId, new GDEContext());
                    });
            }
            else
            {
                await taxiHelper.CreateTaxiOrders(delivery, merchantOrder, link, orderId, context);
            }
        }

        private Task<string> CreateMobileAppOrderLinkForClient(User user, Guid clientOrderId)
        {
            return await mobstedApiHelper.CreateObject(clientDb.User, clientOrderId,
                    MobstedApiHelper.ClientApplicationId, null, null);
        }

        private Task<string> CreateMobileAppOrderLinkForCourier(DeliveryRequest delivery, object merchant)
        {
            return await mobstedApiHelper.CreateObject(
                    new User { Id = orderId, Username = orderId.ToString() }, orderId,
                    MobstedApiHelper.CourierApplicationId, null, null); //create mobile for order not for courier
        }

        private MerchantOrder GetMerchantOrderFromDelivery(DeliveryRequest delivery)
        {
            return JsonConvert.DeserializeObject<MerchantOrder>(delivery.MerchantOrderInfo);
        }

        private void FilterOutDeliveryOptionsAccordingToMerchantSettings(DeliveryRequest delivery, object merchant)
        {
            delivery.Options = delivery.Options.Where(o =>
                         merchant.MaxDeliveryPriceInPenny == null ||
                         o.Price <= merchant.MaxDeliveryPriceInPenny / 100)
                     .ToList();

            if (delivery.Options.Count == 0)
                throw new Exception(
                    $"Цена доставки превышает заданный лимит! лимит: {merchant.MaxDeliveryPriceInPenny / 100}");
        }

        private bool CheckOrderForDeliveryAlreadyExists(Guid deliveryRequestId)
        {
            return context.Set<Repository.Model.Order>().Any(o => o.DeliveryRequestId == deliveryRequestId);
        }

        private Task<OrderDeliveryInfo> CreateExternalOrderFromDraft(OrderInfo order)
        {
            merchantOrder = await convertOrderHelper.ConvertMerchantOrder(order, context, null);
        }

        private Task<Merchant> GetMerchantById(object merchantId)
        {
            return await context.Set<Merchant>().Where(m => m.Id == order.Sender.MerchantId)
                    .FirstOrDefaultAsync();
        }
    }

    internal class GDEContext:IAsyncDisposable
    {
        public GDEContext()
        {
        }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }
    }

    internal class DeliveryRequest
    {
    }

    public class OrderInfo
    {
        public bool IsDraft { get; internal set; }
        public Guid? DeliveryRequestId { get; internal set; }
        public Guid? Id { get; internal set; }
        public Merchant Sender { get; internal set; }
    }

    public class Merchant
    {
        public object MerchantId { get; internal set; }
    }

    public class OrderDeliveryInfo
    {
    }
}
