﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace D365FO_DIXF_ZipPackage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Rest;

    /// <summary>
    /// Extension methods for DMFCreatePackage.
    /// </summary>
    public static partial class DMFCreatePackageExtensions
    {
            /// <summary>
            /// D365FO_DIXF_ZipPackage
            /// </summary>
            /// This function compresses files inside a folder &lt;pathToArchive&gt; to a
            /// ZIP-archive &lt;pathToArchive.zip&gt; and places it  into the folder
            /// &lt;pathToArchive&gt;. The value returned  is the relative path to the
            /// ZIP-archive inside the BLOB container:
            /// &lt;pathToArchive&gt;/&lt;pathToArchive.zip&gt;
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='bLOBContainer'>
            /// Name of BLOB container in the specified Azure Storage Account
            /// </param>
            /// <param name='pathToArchive'>
            /// Name of folder to be archived inside the BLOB container
            /// </param>
            public static void PostD365foDixfZippackage(this IDMFCreatePackage operations, string bLOBContainer, string pathToArchive)
            {
                Task.Factory.StartNew(s => ((IDMFCreatePackage)s).PostD365foDixfZippackageAsync(bLOBContainer, pathToArchive), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// D365FO_DIXF_ZipPackage
            /// </summary>
            /// This function compresses files inside a folder &lt;pathToArchive&gt; to a
            /// ZIP-archive &lt;pathToArchive.zip&gt; and places it  into the folder
            /// &lt;pathToArchive&gt;. The value returned  is the relative path to the
            /// ZIP-archive inside the BLOB container:
            /// &lt;pathToArchive&gt;/&lt;pathToArchive.zip&gt;
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='bLOBContainer'>
            /// Name of BLOB container in the specified Azure Storage Account
            /// </param>
            /// <param name='pathToArchive'>
            /// Name of folder to be archived inside the BLOB container
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task PostD365foDixfZippackageAsync(this IDMFCreatePackage operations, string bLOBContainer, string pathToArchive, CancellationToken cancellationToken = default(CancellationToken))
            {
                await operations.PostD365foDixfZippackageWithHttpMessagesAsync(bLOBContainer, pathToArchive, null, cancellationToken).ConfigureAwait(false);
            }

            /// <summary>
            /// D365FO_DIXF_ZipPackage
            /// </summary>
            /// This function compresses files inside a folder &lt;pathToArchive&gt; to a
            /// ZIP-archive &lt;pathToArchive.zip&gt; and places it  into the folder
            /// &lt;pathToArchive&gt;. The value returned  is the relative path to the
            /// ZIP-archive inside the BLOB container:
            /// &lt;pathToArchive&gt;/&lt;pathToArchive.zip&gt;
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='bLOBContainer'>
            /// Name of BLOB container in the specified Azure Storage Account
            /// </param>
            /// <param name='pathToArchive'>
            /// Name of folder to be archived inside the BLOB container
            /// </param>
            public static void GetD365foDixfZippackage(this IDMFCreatePackage operations, string bLOBContainer, string pathToArchive)
            {
                Task.Factory.StartNew(s => ((IDMFCreatePackage)s).GetD365foDixfZippackageAsync(bLOBContainer, pathToArchive), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// D365FO_DIXF_ZipPackage
            /// </summary>
            /// This function compresses files inside a folder &lt;pathToArchive&gt; to a
            /// ZIP-archive &lt;pathToArchive.zip&gt; and places it  into the folder
            /// &lt;pathToArchive&gt;. The value returned  is the relative path to the
            /// ZIP-archive inside the BLOB container:
            /// &lt;pathToArchive&gt;/&lt;pathToArchive.zip&gt;
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='bLOBContainer'>
            /// Name of BLOB container in the specified Azure Storage Account
            /// </param>
            /// <param name='pathToArchive'>
            /// Name of folder to be archived inside the BLOB container
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task GetD365foDixfZippackageAsync(this IDMFCreatePackage operations, string bLOBContainer, string pathToArchive, CancellationToken cancellationToken = default(CancellationToken))
            {
                await operations.GetD365foDixfZippackageWithHttpMessagesAsync(bLOBContainer, pathToArchive, null, cancellationToken).ConfigureAwait(false);
            }

    }
}
