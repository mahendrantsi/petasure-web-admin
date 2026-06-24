//  <copyright file="EnumRole.cs" company="PlaceholderCompany">
//  Copyright (c) PlaceholderCompany. All rights reserved.
//  </copyright>

namespace Project.Core.Enum
{
    using System.ComponentModel;

    public enum EnumRole
    {
        [Description("Admin")]
        Admin = 1,
        [Description("User")]
        User = 2,
        [Description("SubAdmin")]
        SubAdmin = 3,
        [Description("Seconday User")]
        SecondayUser = 4,
        [Description("Anonymous User")]
        AnonymousUser = 5,
        [Description("Deffra User")]
        DeffraUser = 6,
    }

    public enum EnumUserType
    {
        User = 1,
        InApp = 2
    }
}
