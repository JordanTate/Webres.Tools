using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Core.Connection;

public interface IConnectionValidator
{
    /// <summary>
    /// Validates the specified connection asynchronously.
    /// </summary>
    /// <param name="connection">
    /// The connection information to validate.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the validation operation.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous validation operation, containing the
    /// result of the connection validation.
    /// </returns>
    Task<ConnectionValidationResult> ValidateAsync(
        ConnectionInfo connection,
        CancellationToken cancellationToken);
}
