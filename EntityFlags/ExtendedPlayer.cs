﻿using BrokeProtocol.Entities;

namespace EntityFlags
{
    public class ExtendedPlayer
    {
        public readonly ShPlayer player;
        public ShMountable mountable;
        public byte mountableSeat;
        public string animation;

        public ExtendedPlayer(ShPlayer player) => this.player = player;
    }
}
