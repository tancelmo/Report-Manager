using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Manager.Data;
public class ScheduleData
{
    public int ID
    {
        get; set;
    }
    public string Status
    {
        get; set;
    }

    public string Installation
    {
        get; set;
    }

    public string Invoice
    {
        get; set;
    }

    public string Team
    {
        get; set;
    }
    public string Costumer
    {
        get; set;
    }
    public string MeterSerialNumber
    {
        get; set;
    }
    public string MeterType
    {
        get; set;
    }
    public string Meter
    {
        get; set;
    }
    public string Classification
    {
        get; set;
    }
    public string PTZ
    {
        get; set;
    }
    public string PTZSerialNumber
    {
        get; set;
    }
    public string City
    {
        get; set;
    }
    public string District
    {
        get; set;
    }
    public string Street
    {
        get; set;
    }
    public string Key
    {
        get; set;
    }
    public DateTime Date
    {
        get; set;
    }
    public double Price
    {
        get; set;
    }
    public string Notes
    {
        get; set;
    }

    public bool NotesValidation
    {
        get; set;
    }
    public bool EventsValidation
    {
        get; set;
    }

    public bool MobileValidation
    {
        get; set;
    }
    public bool Bypass
    {
        get; set;
    }
    public bool Emergency
    {
        get; set;
    }

    public string ExpirationDate
    {
        get; set;
    }

    public string MeterTypes
    {
        get; set;
    }

    public string Events
    {
        get; set;
    }
    
}
