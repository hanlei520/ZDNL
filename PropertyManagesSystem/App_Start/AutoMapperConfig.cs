using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Service;

namespace PropertyManagesSystem.App_Start
{
    public class AutoMapperConfig
    {
        public static void Config()
        {
            //注册AutoMapper
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
        }
    }
}