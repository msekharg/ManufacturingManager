function DatatableLookupToReplace(args) {
    /*
     This will be replaced by DatatableLookup when TimesUsed is implemented in the rest of lookup tables
     */
    window.$(document).ready(function () {

        /*debugger;*/;
        var table = "#" + args.tableName;
        var title = args.titleInExcelFile;
        var fileName = args.filenameToExportExcel;
        window.$(table).DataTable({
            dom: 'lfBrtip',
            responsive: true,
            "order": [],
            "columns": [null, null, {orderable:false }],
            "pagingType": "full_numbers",
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            stateSave: true,
            stateDuration: -1,
            "language": {
                "lengthMenu": 'Show <select>' +
                    '<option value="10">10</option>' +
                    '<option value="25">25</option>' +
                    '<option value="50">50</option>' +
                    '<option value="-1">All</option>' +
                    '</select> records.&emsp;&emsp;'
            },

            "buttons": [
                {
                    extend: 'excel',
                    messageTop: title,
                    exportOptions: {
                        columns: [0, 1],
                        format: {
                            body: function (data, row, column, node) {
                                // replace icon for quick case to Yes if exists 
                                if (column === 1) {
                                    if (data.indexOf("bi-x-circle-fill") !== -1) {
                                        data = 'No';
                                    }
                                    else
                                        data = 'Yes';
                                }
                                return data.replace('<div tabindex="0">', '').replace('</div>', '');

                                /*return column === 4 ? data.replace(/[$,]/g, '') : data;*/
                            }
                        }
                    },
                    text: "Export to Excel",
                    attr: {
                        title: 'Export to Excel',
                        role: 'button'
                    },
                    filename: fileName
                }
            ]
        });

    });
}

function DatatableForAdUsers(args) {
    window.$(document).ready(function () {

        debugger;
        var table = "#" + args.tableName;
        var title = args.titleInExcelFile;
        var fileName = args.filenameToExportExcel;
        window.$(table).DataTable({
            dom: 'lfBrtip',
            responsive: true,
            "order": [],
            //"columns": [null, null, null, {orderable:false }],
            "pagingType": "full_numbers",
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            stateSave: true,
            stateDuration: -1,
            "language": {
                "lengthMenu": 'Show <select>' +
                    '<option value="10">10</option>' +
                    '<option value="25">25</option>' +
                    '<option value="50">50</option>' +
                    '<option value="-1">All</option>' +
                    '</select> records.&emsp;&emsp;'
            },

            "buttons": [
                {
                    extend: 'excel',
                    messageTop: title,
                    exportOptions: {
                        columns: [0, 1, 2,3,4] 
                        
                         
                    },
                    text: "Export to Excel",
                    attr: {
                        title: 'Export to Excel',
                        role: 'button'
                    },
                    filename: fileName
                }
            ]
        });

    });
}

function DatatableLookup(args) {
    window.$(document).ready(function () {

        /*debugger;*/; 
        var table = "#" + args.tableName;
        var title = args.titleInExcelFile;
        var fileName = args.filenameToExportExcel;
        window.$(table).DataTable({
            dom : 'lfBrtip',
            responsive: true,
            "order": [],
            //"columns": [null, null, null, {orderable:false }],
            "pagingType": "full_numbers",
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            stateSave: true,
            stateDuration: -1,
            "language": {
                "lengthMenu": 'Show <select>' +
                    '<option value="10">10</option>' +
                    '<option value="25">25</option>' +
                    '<option value="50">50</option>' +
                    '<option value="-1">All</option>' +
                    '</select> records.&emsp;&emsp;'
            },
          
            "buttons": [
                {
                    extend: 'excel',
                    messageTop: title,
                    exportOptions: {
                        columns: [0, 1, 2],
                        format: {
                            body: function (data, row, column, node) {
                                // replace icon for quick case to Yes if exists 
                                if (column === 1) {
                                    if (data.indexOf("bi-x-circle-fill") !== -1) {
                                        data = 'No';
                                    }
                                    else
                                        data = 'Yes';
                                }
                                return data.replace('<div tabindex="0">', '').replace('</div>', '');

                                /*return column === 4 ? data.replace(/[$,]/g, '') : data;*/
                            }
                        }
                    },
                    text: "Export to Excel",
                    attr: {
                        title: 'Export to Excel',
                        role: 'button'
                    },
                    filename:fileName
                }
            ]
        });

    });
}

function DatatableLookup2Col(args) {
    window.$(document).ready(function () {
        /*debugger;*/;
        var table = "#" + args.tableName;
        var title = args.titleInExcelFile;
        var fileName = args.filenameToExportExcel;
        window.$(table).DataTable({
            dom: 'lfBrtip',
            responsive: true,
            "order": [],
            //"columns": [null, null, null, {orderable:false }],
            "pagingType": "full_numbers",
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            stateSave: true,
            stateDuration: -1,
            "language": {
               /* searchPlaceholder: "Search inside table",*/
                "lengthMenu": 'Show <select>' +
                    '<option value="10">10</option>' +
                    '<option value="25">25</option>' +
                    '<option value="50">50</option>' +
                    '<option value="-1">All</option>' +
                    '</select> records.&emsp;&emsp;'
            },

            "buttons": [
                {
                    extend: 'excel',
                    messageTop: title,
                    exportOptions: {
                        columns: [0, 1, 2,3],
                        format: {
                            body: function (data, row, column, node) {
                                // replace icon for quick case to Yes if exists 
                                if (column === 2) {
                                    if (data.indexOf("bi-x-circle-fill") !== -1) {
                                        data = 'No';
                                    }
                                    else
                                        data = 'Yes';
                                }
                                return data.replace('<div tabindex="0">', '').replace('</div>','');

                                /*return column === 4 ? data.replace(/[$,]/g, '') : data;*/
                            }
                        }
                    },
                    text: "Export to Excel",
                    attr: {
                        title: 'Export to Excel',
                        role: 'button'
                    },
                    filename: fileName
                }
            ]
        });

    });
}
function DatatableLog(args) {

    window.$(document).ready(function () {
        /*debugger;*/
        var table ="#"+args.tableName;
        var title = args.titleInExcelFile;
        var fileName = args.filenameToExportExcel;

        window.$(table).DataTable({
            dom: 'lfBrtip',
            responsive: true,
            "order": [],
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            stateSave: true,
            stateDuration: -1,
            "pagingType": "full_numbers",
            "language": {
                "lengthMenu": 'Show <select>' +
                    '<option value="10">10</option>' +
                    '<option value="25">25</option>' +
                    '<option value="50">50</option>' +
                    '<option value="-1">All</option>' +
                    '</select> records.&emsp;&emsp;'
            },
            "buttons": [
                {
                    extend: 'excel',
                    messageTop: title ,
                    exportOptions: {
                        columns: [0, 1, 2]

                    },
                    text: 'Export to Excel',
                    attr: {
                        title: 'Export to Excel',
                        role: 'button'
                    },
                    filename:fileName
                }
            ]
        });

    });
}

//$(".dataTables_length select").selectpicker({
//    width: '180px'
//});
function DatatableListCases(args) {
    window.$(document).ready(function () {
        /*debugger;*/
        var table = "#" + args.tableName;
        var title = args.titleInExcelFile;
        var fileName = args.filenameToExportExcel;
        window.$(table).DataTable({
            dom: 'lfBrtip',
            responsive: true,
            "order": [],
            "pagingType": "full_numbers",
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            "columnDefs": [{ "targets": [3], "type": "MmddYyyy" }],
            "buttons": [
                {
                    extend: 'excel',
                    messageTop: title,
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7,8],
                        orthogonal: "myExport",
                        format: {
                            body: function (data, row, column, node) {
                                // replace icon for quick case to Yes if exists 
                                if (column === 4) {
                                    if (data.indexOf("check-circle-fill")!==-1) {
                                        data='Yes';
                                    }
                                    else
                                    data ='';
                                }
                                return data.replace('<div tabindex="0">', '').replace('</div>', '');
                                
                                /*return column === 4 ? data.replace(/[$,]/g, '') : data;*/
                            }
                        }

                    },
                    text: 'Export to Excel',
                    attr: {
                        title: 'Export to Excel',
                        role: 'button'
                    },
                    filename: fileName
                }
            ],
          
            "language": {
                "lengthMenu": 'Show <select>' +
                    '<option value="10">10</option>' +
                    '<option value="25">25</option>' +
                    '<option value="50">50</option>' +
                    '<option value="-1">All</option>' +
                    '</select> records.&emsp;&emsp;'
            },
            stateSave: true,
            stateDuration:-1

          });
    });
}
function DatatableWorkload(args) {
    window.$(document).ready(function () {
        /*debugger;*/
        var table = "#" + args.tableName;
        var title = args.titleInExcelFile;
        var fileName = args.filenameToExportExcel;
        window.$(table).DataTable({
            dom: 'lfBrtip',
            responsive: true,
            "order": [],
            "pagingType": "full_numbers",
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            "buttons": [
                {
                    extend: 'excel',
                    messageTop: title,
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7],
                        orthogonal: "myExport",
                        format: {
                            body: function (data, row, column, node) {
                                // replace icon for quick case to Yes if exists 
                                if (column === 4) {
                                    if (data.indexOf("check-circle-fill") !== -1) {
                                        data = 'Yes';
                                    }
                                    else
                                        data = '';
                                }
                                return data.replace('<div tabindex="0">', '').replace('</div>', '');

                                /*return column === 4 ? data.replace(/[$,]/g, '') : data;*/
                            }
                        }

                    },
                    text: 'Export to Excel',
                    attr: {
                        title: 'Export to Excel',
                        role: 'button'
                    },
                    filename: fileName
                }
            ],
            //"columns": [
            //    { "data": "CaseName"},
            //    { "data": "CustomerName"},
            //    { "data": "Email"},
            //    { "data": "DateCreated"},
            //    {
            //        "data": "QuickCase", "width": "10%",
            //        "searchable": false, "className": "text-center",
            //        "render": function (data, type, row) {
            //            if (type === 'myExport') {
            //                return data === 1 ? "Y" : "N";
            //            }
            //            if (type === 'display') {
            //                if (data === 1) {
            //                    return '<i class="bi bi-check-circle-fill" role="img" aria-label="This is a quick case" Title="This is a quick case" style="font-size: 1rem; color: green;" ></i>';
            //                } else {
            //                    return '<i class="bi bi-x-circle" role="img" aria-label="Inactive" Title="Inactive" style="font-size: 1rem; color: red;" ></i>';
            //                }
            //            }
            //            return data;
            //        }
            //    },
            //    { "data": "CaseDescription"},
            //    { "data": "CategoryDescription"},
            //    { "data": "PriorityDescription" }
            //],
            "language": {
                "lengthMenu": 'Show <select>' +
                    '<option value="10">10</option>' +
                    '<option value="25">25</option>' +
                    '<option value="50">50</option>' +
                    '<option value="-1">All</option>' +
                    '</select> records.&emsp;&emsp;'
            },
            stateSave: true,
            stateDuration: -1

        });
    });
}

function DatatableDimensionWorkload(args) {
    window.$(document).ready(function () {
        /*debugger;*/
        var table = "#" + args.tableName;
        var title = args.titleInExcelFile;
        var fileName = args.filenameToExportExcel;
        window.$(table).DataTable({
            dom: 'lfBrtip',
            responsive: true,
            "order": [],
            "pagingType": "full_numbers",
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            "buttons": [
                {
                    extend: 'excel',
                    messageTop: title,
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4],
                        orthogonal: "myExport",
                        format: {
                            body: function (data, row, column, node) {
                                // replace icon for quick case to Yes if exists 
                                if (column === 4) {
                                    if (data.indexOf("check-circle-fill") !== -1) {
                                        data = 'Yes';
                                    }
                                    else
                                        data = '';
                                }
                                return data.replace('<div tabindex="0">', '').replace('</div>', '');

                                /*return column === 4 ? data.replace(/[$,]/g, '') : data;*/
                            }
                        }

                    },
                    text: 'Export to Excel',
                    attr: {
                        title: 'Export to Excel',
                        role: 'button'
                    },
                    filename: fileName
                }
            ],
            //"columns": [
            //    { "data": "CaseName"},
            //    { "data": "CustomerName"},
            //    { "data": "Email"},
            //    { "data": "DateCreated"},
            //    {
            //        "data": "QuickCase", "width": "10%",
            //        "searchable": false, "className": "text-center",
            //        "render": function (data, type, row) {
            //            if (type === 'myExport') {
            //                return data === 1 ? "Y" : "N";
            //            }
            //            if (type === 'display') {
            //                if (data === 1) {
            //                    return '<i class="bi bi-check-circle-fill" role="img" aria-label="This is a quick case" Title="This is a quick case" style="font-size: 1rem; color: green;" ></i>';
            //                } else {
            //                    return '<i class="bi bi-x-circle" role="img" aria-label="Inactive" Title="Inactive" style="font-size: 1rem; color: red;" ></i>';
            //                }
            //            }
            //            return data;
            //        }
            //    },
            //    { "data": "CaseDescription"},
            //    { "data": "CategoryDescription"},
            //    { "data": "PriorityDescription" }
            //],
            "language": {
                "lengthMenu": 'Show <select>' +
                    '<option value="10">10</option>' +
                    '<option value="25">25</option>' +
                    '<option value="50">50</option>' +
                    '<option value="-1">All</option>' +
                    '</select> records.&emsp;&emsp;'
            },
            stateSave: true,
            stateDuration: -1

        });
    });
}

function DatatableSolarInspectionWorkload(args) {
    window.$(document).ready(function () {
        /*debugger;*/
        var table = "#" + args.tableName;
        var title = args.titleInExcelFile;
        var fileName = args.filenameToExportExcel;
        window.$(table).DataTable({
            dom: 'lfBrtip',
            responsive: true,
            "order": [],
            "pagingType": "full_numbers",
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            "buttons": [
                {
                    extend: 'excel',
                    messageTop: title,
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4],
                        orthogonal: "myExport",
                        format: {
                            body: function (data, row, column, node) {
                                // replace icon for quick case to Yes if exists 
                                if (column === 4) {
                                    if (data.indexOf("check-circle-fill") !== -1) {
                                        data = 'Yes';
                                    }
                                    else
                                        data = '';
                                }
                                return data.replace('<div tabindex="0">', '').replace('</div>', '');

                                /*return column === 4 ? data.replace(/[$,]/g, '') : data;*/
                            }
                        }

                    },
                    text: 'Export to Excel',
                    attr: {
                        title: 'Export to Excel',
                        role: 'button'
                    },
                    filename: fileName
                }
            ],
            //"columns": [
            //    { "data": "CaseName"},
            //    { "data": "CustomerName"},
            //    { "data": "Email"},
            //    { "data": "DateCreated"},
            //    {
            //        "data": "QuickCase", "width": "10%",
            //        "searchable": false, "className": "text-center",
            //        "render": function (data, type, row) {
            //            if (type === 'myExport') {
            //                return data === 1 ? "Y" : "N";
            //            }
            //            if (type === 'display') {
            //                if (data === 1) {
            //                    return '<i class="bi bi-check-circle-fill" role="img" aria-label="This is a quick case" Title="This is a quick case" style="font-size: 1rem; color: green;" ></i>';
            //                } else {
            //                    return '<i class="bi bi-x-circle" role="img" aria-label="Inactive" Title="Inactive" style="font-size: 1rem; color: red;" ></i>';
            //                }
            //            }
            //            return data;
            //        }
            //    },
            //    { "data": "CaseDescription"},
            //    { "data": "CategoryDescription"},
            //    { "data": "PriorityDescription" }
            //],
            "language": {
                "lengthMenu": 'Show <select>' +
                    '<option value="10">10</option>' +
                    '<option value="25">25</option>' +
                    '<option value="50">50</option>' +
                    '<option value="-1">All</option>' +
                    '</select> records.&emsp;&emsp;'
            },
            stateSave: true,
            stateDuration: -1

        });
    });
}

function DatatableInProcessCheckWorkload(args) {
    window.$(document).ready(function () {
        /*debugger;*/
        var table = "#" + args.tableName;
        var title = args.titleInExcelFile;
        var fileName = args.filenameToExportExcel;
        window.$(table).DataTable({
            dom: 'lfBrtip',
            responsive: true,
            "order": [],
            "pagingType": "full_numbers",
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            "buttons": [
                {
                    extend: 'excel',
                    messageTop: title,
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7],
                        orthogonal: "myExport",
                        format: {
                            body: function (data, row, column, node) {
                                // replace icon for quick case to Yes if exists 
                                if (column === 7) {
                                    if (data.indexOf("check-circle-fill") !== -1) {
                                        data = 'Yes';
                                    }
                                    else
                                        data = '';
                                }
                                return data.replace('<div tabindex="0">', '').replace('</div>', '');

                                /*return column === 4 ? data.replace(/[$,]/g, '') : data;*/
                            }
                        }

                    },
                    text: 'Export to Excel',
                    attr: {
                        title: 'Export to Excel',
                        role: 'button'
                    },
                    filename: fileName
                }
            ],
            //"columns": [
            //    { "data": "CaseName"},
            //    { "data": "CustomerName"},
            //    { "data": "Email"},
            //    { "data": "DateCreated"},
            //    {
            //        "data": "QuickCase", "width": "10%",
            //        "searchable": false, "className": "text-center",
            //        "render": function (data, type, row) {
            //            if (type === 'myExport') {
            //                return data === 1 ? "Y" : "N";
            //            }
            //            if (type === 'display') {
            //                if (data === 1) {
            //                    return '<i class="bi bi-check-circle-fill" role="img" aria-label="This is a quick case" Title="This is a quick case" style="font-size: 1rem; color: green;" ></i>';
            //                } else {
            //                    return '<i class="bi bi-x-circle" role="img" aria-label="Inactive" Title="Inactive" style="font-size: 1rem; color: red;" ></i>';
            //                }
            //            }
            //            return data;
            //        }
            //    },
            //    { "data": "CaseDescription"},
            //    { "data": "CategoryDescription"},
            //    { "data": "PriorityDescription" }
            //],
            "language": {
                "lengthMenu": 'Show <select>' +
                    '<option value="10">10</option>' +
                    '<option value="25">25</option>' +
                    '<option value="50">50</option>' +
                    '<option value="-1">All</option>' +
                    '</select> records.&emsp;&emsp;'
            },
            stateSave: true,
            stateDuration: -1

        });
    });
}

function DatatableFinalInspectionWorkload(args) {
    window.$(document).ready(function () {
        /*debugger;*/
        var table = "#" + args.tableName;
        var title = args.titleInExcelFile;
        var fileName = args.filenameToExportExcel;
        window.$(table).DataTable({
            dom: 'lfBrtip',
            responsive: true,
            "order": [],
            "pagingType": "full_numbers",
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            "buttons": [
                {
                    extend: 'excel',
                    messageTop: title,
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4],
                        orthogonal: "myExport",
                        format: {
                            body: function (data, row, column, node) {
                                // replace icon for quick case to Yes if exists 
                                if (column === 4) {
                                    if (data.indexOf("check-circle-fill") !== -1) {
                                        data = 'Yes';
                                    }
                                    else
                                        data = '';
                                }
                                return data.replace('<div tabindex="0">', '').replace('</div>', '');

                                /*return column === 4 ? data.replace(/[$,]/g, '') : data;*/
                            }
                        }

                    },
                    text: 'Export to Excel',
                    attr: {
                        title: 'Export to Excel',
                        role: 'button'
                    },
                    filename: fileName
                }
            ],
            //"columns": [
            //    { "data": "CaseName"},
            //    { "data": "CustomerName"},
            //    { "data": "Email"},
            //    { "data": "DateCreated"},
            //    {
            //        "data": "QuickCase", "width": "10%",
            //        "searchable": false, "className": "text-center",
            //        "render": function (data, type, row) {
            //            if (type === 'myExport') {
            //                return data === 1 ? "Y" : "N";
            //            }
            //            if (type === 'display') {
            //                if (data === 1) {
            //                    return '<i class="bi bi-check-circle-fill" role="img" aria-label="This is a quick case" Title="This is a quick case" style="font-size: 1rem; color: green;" ></i>';
            //                } else {
            //                    return '<i class="bi bi-x-circle" role="img" aria-label="Inactive" Title="Inactive" style="font-size: 1rem; color: red;" ></i>';
            //                }
            //            }
            //            return data;
            //        }
            //    },
            //    { "data": "CaseDescription"},
            //    { "data": "CategoryDescription"},
            //    { "data": "PriorityDescription" }
            //],
            "language": {
                "lengthMenu": 'Show <select>' +
                    '<option value="10">10</option>' +
                    '<option value="25">25</option>' +
                    '<option value="50">50</option>' +
                    '<option value="-1">All</option>' +
                    '</select> records.&emsp;&emsp;'
            },
            stateSave: true,
            stateDuration: -1

        });
    });
}

function UsersTableLookup(args) {
    window.$(document).ready(function () {
        var table = "#" + args.tableName;
        var title = args.titleInExcelFile;
        var fileName = args.filenameToExportExcel;
        window.$(table).DataTable({
            dom: 'lfBrtip',
            responsive: true,
            "order": [],
            //"columns": [null, null, null,null,null,null, null, {orderable:false }, {orderable:false }],
            "pagingType": "full_numbers",
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            stateSave: true,
            stateDuration: -1,
            "language": {
                "lengthMenu": 'Show <select>' +
                    '<option value="10">10</option>' +
                    '<option value="25">25</option>' +
                    '<option value="50">50</option>' +
                    '<option value="-1">All</option>' +
                    '</select> records.&emsp;&emsp;'
            },

            "buttons": [
                {
                    extend: 'excel',
                    messageTop: title,
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7],
                        format: {
                            body: function (data, row, column, node) {
                                // replace icon for quick case to Yes if exists 
                                if (column === 7) {
                                    if (data.indexOf("bi-x-circle-fill") !== -1) {
                                        data = 'No';
                                    }
                                    else
                                        data = 'Yes';
                                }
                                return data.replace('<div tabindex="0">', '').replace('</div>', '');

                                /*return column === 4 ? data.replace(/[$,]/g, '') : data;*/
                            }
                        }
                    },
                    text: "Export to Excel",
                    attr: {
                        title: 'Export to Excel',
                        role: 'button'
                    },
                    filename: fileName
                }
            ]
        });

    });
}


function DataTableRemove(table) {
    /*debugger;*/
    $(document).ready(function() {
        $(table).DataTable().destroy();
    });
}

function OpenWindow(url) {
    /*window.open('https://www.google.com', 'Test', 'width=800,height=860,scrollbars=no,toolbar=no,location=no');*/
   // window.open(url);
    var backlen = history.lenght;
    history.go(-backlen);
    return;
}
