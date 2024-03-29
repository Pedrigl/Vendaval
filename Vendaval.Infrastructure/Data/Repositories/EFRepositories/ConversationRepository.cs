﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Entities;
using Vendaval.Infrastructure.Data.Contexts;
using Vendaval.Infrastructure.Data.Repositories.EFRepositories.Interfaces;

namespace Vendaval.Infrastructure.Data.Repositories.EFRepositories
{
    public class ConversationRepository : EFGenericRepository<Conversation>, IConversationRepository
    {
        private readonly VendavalDbContext _context;

        public ConversationRepository(VendavalDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Conversation> GetConversationByParticipantsAsync(int user1Id, int user2Id)
        {
            return await _context.Conversations.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Participants.Any(p => p.Id == user1Id) && c.Participants.Any(p => p.Id == user2Id));
        }

        public async Task<List<Conversation>> GetConversationsByUserIdAsync(int userId)
        {
            return await _context.Conversations.AsNoTracking()
                .Where(c => c.Participants.Any(p => p.Id == userId))
                .Include(c => c.Participants)
                .Include(c => c.Messages)
                .ToListAsync();
        }

        public async Task AddMessageToConversation(int conversationId, Message message)
        {
            var conversation = await _context.Conversations
                                            .Include(c => c.Messages).FirstOrDefaultAsync(c => c.Id == conversationId);
            conversation.Messages.Add(message);
        }


    }
}
