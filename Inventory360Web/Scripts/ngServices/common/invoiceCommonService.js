myApp
    .factory('invoiceCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchAllInvoice = function (q, CustomerId, dateFrom, dateTo) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00212'
                        + '?query=' + q
                        + '&CustomerId=' + CustomerId
                        + '&dateFrom=' + dateFrom
                        + '&dateTo=' + dateTo
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            fac.FetchAllInvoiceShortInfo = function (id) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00213'
                        + '?id=' + id
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            fac.FetchProductNameByInvoice = function (q, InvoiceId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00214'
                        + '?query=' + q
                        + '&InvoiceId=' + InvoiceId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            fac.FetchSalesInvoiceInfoByProduct = function (ProductId, productSerial) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00215'
                        + '?ProductId=' + ProductId
                        + '&ProductSerial=' + productSerial
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            fac.FetchSalesInvoiceWarrantyInfoByProduct = function (InvoiceId, ProductId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00216'
                        + '?InvoiceId=' + InvoiceId
                        + '&ProductId=' + ProductId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            fac.FetchProductBySerialFromInvoice = function (q, isReceiveAgainstPreviousSales, ProductId, InvoiceId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C00217'
                        + '?serial=' + q
                        + '&isReceiveAgainstPreviousSales=' + isReceiveAgainstPreviousSales
                        + '&ProductId=' + ProductId
                        + '&InvoiceId=' + InvoiceId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            return fac;
        }
    ]);