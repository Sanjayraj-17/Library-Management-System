using System;
using System.Collections.Generic;
using System.Text;


using LibraryManagementEFCore.Services;

namespace LibraryManagementEFCore.Menus;

public class DashboardMenu
{
    private readonly DashboardService _service;

    public DashboardMenu(DashboardService service)
    {
        _service = service;
    }

    public async Task RunAsync()
    {
        await _service.ShowDashboardAsync();
    }
}
