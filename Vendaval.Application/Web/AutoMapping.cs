using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Application.ViewModels;
using Vendaval.Domain.Entities;

namespace Vendaval.Application.Web
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductViewModel, Product>();
            CreateMap<ProductAvaliation, ProductAvaliationViewModel>();
            CreateMap<ProductAvaliationViewModel, ProductAvaliation>();
            CreateMap<Order, OrderViewModel>();
            CreateMap<OrderViewModel, Order>();
            CreateMap<User, ChatUserViewModel>();
            CreateMap<ChatUserViewModel, ChatUser>();
            CreateMap<ChatUser, ChatUserViewModel>();
            CreateMap<Conversation, ConversationViewModel>();
            CreateMap<ConversationViewModel, Conversation>();
            CreateMap<Message, MessageViewModel>();
            CreateMap<MessageViewModel, Message>();
        }
    }
}
