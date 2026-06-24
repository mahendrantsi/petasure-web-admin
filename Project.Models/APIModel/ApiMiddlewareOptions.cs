// <copyright file="ApiMiddlewareOptions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Project.Models.APIModel
{
    public class ApiMiddlewareOptions
    {
        public string GenericMessage { get; set; }

        // / <summary>
        // / Gets or sets the response format exclude - to avoid reformatting the response (eg, if returning bytes)
        // / </summary>
        // / <value>
        // / The response format exclude.
        // / </value>
        public string[] ResponseFormatExclude { get; set; }
    }
}
