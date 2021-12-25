using System;

namespace Hydra.Component.Authorization
{
    public class AuthOptions
    {
        public Uri ApiBaseUrl { get; set; }

        public Endpoints Endpoints { get; set; } = new();

    }
}