using System;
using System.Collections.Generic;
using System.Text;

namespace SQLDocUI
{
    public enum ObjectTypes
    {
        Triggers,
        UserDefinedDataTypes,
        InstanceInformation,
        DataSourceInformation,
        DataTypes,
        Restrictions,
        ReservedWords,
        Users,
        Columns,
        ForeignKeys,
        IndexColumns,
        Indexes,
        Tables,
        ViewColumns,
        Views,
        ProcedureParameters,
        Procedures
    }

    public enum OSVersion
    {
        Windows3_1,
        Windows95,
        Windows98SecondEdition,
        Windows98,
        WindowsME,
        WindowsNT3_51,
        WindowsNT4_0,
        Windows2000,
        WindowsXP,
        Windows2003,
        WindowsVista,
        Windows7,
        WindowsCE,
        Unix,
        Unknown
    }
}
