using System;
using Common.Interfaces;
using Domain.Models;

namespace FriendService.Contracts
{
    public class FollowResponse : ResponseBase
    {
        public Follow Follow { get; internal set; }
    }
}

