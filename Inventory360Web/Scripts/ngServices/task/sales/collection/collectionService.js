myApp
    .factory('collectionService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.GetCollectionMappingData = function (collectionAgainst, currency, customerId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/TS0021'
                        + '?customerId=' + customerId
                        + '&currency=' + currency
                        + '&collectionAgainst=' + collectionAgainst
                    //+ '#' + Math.random().toString(36)
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.SaveSalesCollection = function (
                collectionDate, selectedCurrency, exchangeRate,
                collectedAmount, customerId, salesPersonId,
                collectedBy, moneyReceiptNo, remarks,
                detailLists, mappingLists
            ) {
                return $http({
                    method: 'POST',
                    url: serviceBasePath + '/api/TI009',
                    dataType: 'json',
                    data: JSON.stringify({
                        "CollectionDate": collectionDate,
                        "SelectedCurrency": selectedCurrency,
                        "ExchangeRate": exchangeRate,
                        "CollectedAmount": collectedAmount,
                        "CustomerId": customerId,
                        "SalesPersonId": salesPersonId,
                        "CollectedBy": collectedBy,
                        "MRNo": moneyReceiptNo,
                        "Remarks": remarks,
                        "CollectionDetailLists": detailLists,
                        "CollectionMappingLists": mappingLists
                    }),
                    headers: { "Content-Type": "application/json" }
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);