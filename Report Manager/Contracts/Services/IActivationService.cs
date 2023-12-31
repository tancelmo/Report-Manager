﻿namespace Report_Manager.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);

    Task ActivateAsyncMain(object activationArgs);
}
