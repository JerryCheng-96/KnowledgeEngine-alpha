var ke_app = angular.module('KEApp', ['nvd3']);

ke_app.controller('TabCtrl', function ($scope, $rootScope, $http) {
    $rootScope.nowActBtn = 1;
    $scope.navBarClick = function (numberBtn) {
        $rootScope.nowActBtn = numberBtn;
        console.log($rootScope.nowActBtn);
    };
    $rootScope.$broadcast('buttonChanged');
});

ke_app.controller('myCtrl', function ($rootScope, $scope) {
    var color = d3.scale.category20();
    $scope.options = {
        chart: {
            "type": 'forceDirectedGraph',
            "height": 525,
            "width": 1000,
            "margin": {
                "top": 20,
                "right": 20,
                "bottom": 20,
                "left": 20
            },
            "color": function (d) {
                return color(d.group)
            },
            nodeExtras: function (node) {
                node && node
                    .append("text")
                    .text(function (d) { return d.show })
                    .style('font-size', '10px');
            }
        }
    };

    $rootScope.data = {
        "nodes": [
            { "name": "Myriel", "group": 1 },
            { "name": "Napoleon", "group": 1 },
            { "name": "Mlle.Baptistine", "group": 1 },
            { "name": "Mme.Magloire", "group": 1 },
            { "name": "CountessdeLo", "group": 1 },
            { "name": "Geborand", "group": 1 },
            { "name": "Champtercier", "group": 1 },
            { "name": "Cravatte", "group": 1 },
            { "name": "Count", "group": 1 },
            { "name": "OldMan", "group": 1 },
            { "name": "Labarre", "group": 2 },
            { "name": "Valjean", "group": 2 },
            { "name": "Marguerite", "group": 3 },
            { "name": "Mme.deR", "group": 2 },
            { "name": "Isabeau", "group": 2 },
            { "name": "Gervais", "group": 2 },
            { "name": "Tholomyes", "group": 3 },
            { "name": "Listolier", "group": 3 },
            { "name": "Fameuil", "group": 3 },
            { "name": "Blacheville", "group": 3 },
            { "name": "Favourite", "group": 3 },
            { "name": "Dahlia", "group": 3 },
            { "name": "Zephine", "group": 3 },
            { "name": "Fantine", "group": 3 },
            { "name": "Mme.Thenardier", "group": 4 },
            { "name": "Thenardier", "group": 4 },
            { "name": "Cosette", "group": 5 },
            { "name": "Javert", "group": 4 },
            { "name": "Fauchelevent", "group": 0 },
            { "name": "Bamatabois", "group": 2 },
            { "name": "Perpetue", "group": 3 },
            { "name": "Simplice", "group": 2 },
            { "name": "Scaufflaire", "group": 2 },
            { "name": "Woman1", "group": 2 },
            { "name": "Judge", "group": 2 },
            { "name": "Champmathieu", "group": 2 },
            { "name": "Brevet", "group": 2 },
            { "name": "Chenildieu", "group": 2 },
            { "name": "Cochepaille", "group": 2 },
            { "name": "Pontmercy", "group": 4 },
            { "name": "Boulatruelle", "group": 6 },
            { "name": "Eponine", "group": 4 },
            { "name": "Anzelma", "group": 4 },
            { "name": "Woman2", "group": 5 },
            { "name": "MotherInnocent", "group": 0 },
            { "name": "Gribier", "group": 0 },
            { "name": "Jondrette", "group": 7 },
            { "name": "Mme.Burgon", "group": 7 },
            { "name": "Gavroche", "group": 8 },
            { "name": "Gillenormand", "group": 5 },
            { "name": "Magnon", "group": 5 },
            { "name": "Mlle.Gillenormand", "group": 5 },
            { "name": "Mme.Pontmercy", "group": 5 },
            { "name": "Mlle.Vaubois", "group": 5 },
            { "name": "Lt.Gillenormand", "group": 5 },
            { "name": "Marius", "group": 8 },
            { "name": "BaronessT", "group": 5 },
            { "name": "Mabeuf", "group": 8 },
            { "name": "Enjolras", "group": 8 },
            { "name": "Combeferre", "group": 8 },
            { "name": "Prouvaire", "group": 8 },
            { "name": "Feuilly", "group": 8 },
            { "name": "Courfeyrac", "group": 8 },
            { "name": "Bahorel", "group": 8 },
            { "name": "Bossuet", "group": 8 },
            { "name": "Joly", "group": 8 },
            { "name": "Grantaire", "group": 8 },
            { "name": "MotherPlutarch", "group": 9 },
            { "name": "Gueulemer", "group": 4 },
            { "name": "Babet", "group": 4 },
            { "name": "Claquesous", "group": 4 },
            { "name": "Montparnasse", "group": 4 },
            { "name": "Toussaint", "group": 5 },
            { "name": "Child1", "group": 10 },
            { "name": "Child2", "group": 10 },
            { "name": "Brujon", "group": 4 },
            { "name": "Mme.Hucheloup", "group": 8 }
        ],
        "links": [
            { "source": 1, "target": 0, "value": 1 },
            { "source": 2, "target": 0, "value": 8 },
            { "source": 3, "target": 0, "value": 10 },
            { "source": 3, "target": 2, "value": 6 },
            { "source": 4, "target": 0, "value": 1 },
            { "source": 5, "target": 0, "value": 1 },
            { "source": 6, "target": 0, "value": 1 },
            { "source": 7, "target": 0, "value": 1 },
            { "source": 8, "target": 0, "value": 2 },
            { "source": 9, "target": 0, "value": 1 },
            { "source": 11, "target": 10, "value": 1 },
            { "source": 11, "target": 3, "value": 3 },
            { "source": 11, "target": 2, "value": 3 },
            { "source": 11, "target": 0, "value": 5 },
            { "source": 12, "target": 11, "value": 1 },
            { "source": 13, "target": 11, "value": 1 },
            { "source": 14, "target": 11, "value": 1 },
            { "source": 15, "target": 11, "value": 1 },
            { "source": 17, "target": 16, "value": 4 },
            { "source": 18, "target": 16, "value": 4 },
            { "source": 18, "target": 17, "value": 4 },
            { "source": 19, "target": 16, "value": 4 },
            { "source": 19, "target": 17, "value": 4 },
            { "source": 19, "target": 18, "value": 4 },
            { "source": 20, "target": 16, "value": 3 },
            { "source": 20, "target": 17, "value": 3 },
            { "source": 20, "target": 18, "value": 3 },
            { "source": 20, "target": 19, "value": 4 },
            { "source": 21, "target": 16, "value": 3 },
            { "source": 21, "target": 17, "value": 3 },
            { "source": 21, "target": 18, "value": 3 },
            { "source": 21, "target": 19, "value": 3 },
            { "source": 21, "target": 20, "value": 5 },
            { "source": 22, "target": 16, "value": 3 },
            { "source": 22, "target": 17, "value": 3 },
            { "source": 22, "target": 18, "value": 3 },
            { "source": 22, "target": 19, "value": 3 },
            { "source": 22, "target": 20, "value": 4 },
            { "source": 22, "target": 21, "value": 4 },
            { "source": 23, "target": 16, "value": 3 },
            { "source": 23, "target": 17, "value": 3 },
            { "source": 23, "target": 18, "value": 3 },
            { "source": 23, "target": 19, "value": 3 },
            { "source": 23, "target": 20, "value": 4 },
            { "source": 23, "target": 21, "value": 4 },
            { "source": 23, "target": 22, "value": 4 },
            { "source": 23, "target": 12, "value": 2 },
            { "source": 23, "target": 11, "value": 9 },
            { "source": 24, "target": 23, "value": 2 },
            { "source": 24, "target": 11, "value": 7 },
            { "source": 25, "target": 24, "value": 13 },
            { "source": 25, "target": 23, "value": 1 },
            { "source": 25, "target": 11, "value": 12 },
            { "source": 26, "target": 24, "value": 4 },
            { "source": 26, "target": 11, "value": 31 },
            { "source": 26, "target": 16, "value": 1 },
            { "source": 26, "target": 25, "value": 1 },
            { "source": 27, "target": 11, "value": 17 },
            { "source": 27, "target": 23, "value": 5 },
            { "source": 27, "target": 25, "value": 5 },
            { "source": 27, "target": 24, "value": 1 },
            { "source": 27, "target": 26, "value": 1 },
            { "source": 28, "target": 11, "value": 8 },
            { "source": 28, "target": 27, "value": 1 },
            { "source": 29, "target": 23, "value": 1 },
            { "source": 29, "target": 27, "value": 1 },
            { "source": 29, "target": 11, "value": 2 },
            { "source": 30, "target": 23, "value": 1 },
            { "source": 31, "target": 30, "value": 2 },
            { "source": 31, "target": 11, "value": 3 },
            { "source": 31, "target": 23, "value": 2 },
            { "source": 31, "target": 27, "value": 1 },
            { "source": 32, "target": 11, "value": 1 },
            { "source": 33, "target": 11, "value": 2 },
            { "source": 33, "target": 27, "value": 1 },
            { "source": 34, "target": 11, "value": 3 },
            { "source": 34, "target": 29, "value": 2 },
            { "source": 35, "target": 11, "value": 3 },
            { "source": 35, "target": 34, "value": 3 },
            { "source": 35, "target": 29, "value": 2 },
            { "source": 36, "target": 34, "value": 2 },
            { "source": 36, "target": 35, "value": 2 },
            { "source": 36, "target": 11, "value": 2 },
            { "source": 36, "target": 29, "value": 1 },
            { "source": 37, "target": 34, "value": 2 },
            { "source": 37, "target": 35, "value": 2 },
            { "source": 37, "target": 36, "value": 2 },
            { "source": 37, "target": 11, "value": 2 },
            { "source": 37, "target": 29, "value": 1 },
            { "source": 38, "target": 34, "value": 2 },
            { "source": 38, "target": 35, "value": 2 },
            { "source": 38, "target": 36, "value": 2 },
            { "source": 38, "target": 37, "value": 2 },
            { "source": 38, "target": 11, "value": 2 },
            { "source": 38, "target": 29, "value": 1 },
            { "source": 39, "target": 25, "value": 1 },
            { "source": 40, "target": 25, "value": 1 },
            { "source": 41, "target": 24, "value": 2 },
            { "source": 41, "target": 25, "value": 3 },
            { "source": 42, "target": 41, "value": 2 },
            { "source": 42, "target": 25, "value": 2 },
            { "source": 42, "target": 24, "value": 1 },
            { "source": 43, "target": 11, "value": 3 },
            { "source": 43, "target": 26, "value": 1 },
            { "source": 43, "target": 27, "value": 1 },
            { "source": 44, "target": 28, "value": 3 },
            { "source": 44, "target": 11, "value": 1 },
            { "source": 45, "target": 28, "value": 2 },
            { "source": 47, "target": 46, "value": 1 },
            { "source": 48, "target": 47, "value": 2 },
            { "source": 48, "target": 25, "value": 1 },
            { "source": 48, "target": 27, "value": 1 },
            { "source": 48, "target": 11, "value": 1 },
            { "source": 49, "target": 26, "value": 3 },
            { "source": 49, "target": 11, "value": 2 },
            { "source": 50, "target": 49, "value": 1 },
            { "source": 50, "target": 24, "value": 1 },
            { "source": 51, "target": 49, "value": 9 },
            { "source": 51, "target": 26, "value": 2 },
            { "source": 51, "target": 11, "value": 2 },
            { "source": 52, "target": 51, "value": 1 },
            { "source": 52, "target": 39, "value": 1 },
            { "source": 53, "target": 51, "value": 1 },
            { "source": 54, "target": 51, "value": 2 },
            { "source": 54, "target": 49, "value": 1 },
            { "source": 54, "target": 26, "value": 1 },
            { "source": 55, "target": 51, "value": 6 },
            { "source": 55, "target": 49, "value": 12 },
            { "source": 55, "target": 39, "value": 1 },
            { "source": 55, "target": 54, "value": 1 },
            { "source": 55, "target": 26, "value": 21 },
            { "source": 55, "target": 11, "value": 19 },
            { "source": 55, "target": 16, "value": 1 },
            { "source": 55, "target": 25, "value": 2 },
            { "source": 55, "target": 41, "value": 5 },
            { "source": 55, "target": 48, "value": 4 },
            { "source": 56, "target": 49, "value": 1 },
            { "source": 56, "target": 55, "value": 1 },
            { "source": 57, "target": 55, "value": 1 },
            { "source": 57, "target": 41, "value": 1 },
            { "source": 57, "target": 48, "value": 1 },
            { "source": 58, "target": 55, "value": 7 },
            { "source": 58, "target": 48, "value": 7 },
            { "source": 58, "target": 27, "value": 6 },
            { "source": 58, "target": 57, "value": 1 },
            { "source": 58, "target": 11, "value": 4 },
            { "source": 59, "target": 58, "value": 15 },
            { "source": 59, "target": 55, "value": 5 },
            { "source": 59, "target": 48, "value": 6 },
            { "source": 59, "target": 57, "value": 2 },
            { "source": 60, "target": 48, "value": 1 },
            { "source": 60, "target": 58, "value": 4 },
            { "source": 60, "target": 59, "value": 2 },
            { "source": 61, "target": 48, "value": 2 },
            { "source": 61, "target": 58, "value": 6 },
            { "source": 61, "target": 60, "value": 2 },
            { "source": 61, "target": 59, "value": 5 },
            { "source": 61, "target": 57, "value": 1 },
            { "source": 61, "target": 55, "value": 1 },
            { "source": 62, "target": 55, "value": 9 },
            { "source": 62, "target": 58, "value": 17 },
            { "source": 62, "target": 59, "value": 13 },
            { "source": 62, "target": 48, "value": 7 },
            { "source": 62, "target": 57, "value": 2 },
            { "source": 62, "target": 41, "value": 1 },
            { "source": 62, "target": 61, "value": 6 },
            { "source": 62, "target": 60, "value": 3 },
            { "source": 63, "target": 59, "value": 5 },
            { "source": 63, "target": 48, "value": 5 },
            { "source": 63, "target": 62, "value": 6 },
            { "source": 63, "target": 57, "value": 2 },
            { "source": 63, "target": 58, "value": 4 },
            { "source": 63, "target": 61, "value": 3 },
            { "source": 63, "target": 60, "value": 2 },
            { "source": 63, "target": 55, "value": 1 },
            { "source": 64, "target": 55, "value": 5 },
            { "source": 64, "target": 62, "value": 12 },
            { "source": 64, "target": 48, "value": 5 },
            { "source": 64, "target": 63, "value": 4 },
            { "source": 64, "target": 58, "value": 10 },
            { "source": 64, "target": 61, "value": 6 },
            { "source": 64, "target": 60, "value": 2 },
            { "source": 64, "target": 59, "value": 9 },
            { "source": 64, "target": 57, "value": 1 },
            { "source": 64, "target": 11, "value": 1 },
            { "source": 65, "target": 63, "value": 5 },
            { "source": 65, "target": 64, "value": 7 },
            { "source": 65, "target": 48, "value": 3 },
            { "source": 65, "target": 62, "value": 5 },
            { "source": 65, "target": 58, "value": 5 },
            { "source": 65, "target": 61, "value": 5 },
            { "source": 65, "target": 60, "value": 2 },
            { "source": 65, "target": 59, "value": 5 },
            { "source": 65, "target": 57, "value": 1 },
            { "source": 65, "target": 55, "value": 2 },
            { "source": 66, "target": 64, "value": 3 },
            { "source": 66, "target": 58, "value": 3 },
            { "source": 66, "target": 59, "value": 1 },
            { "source": 66, "target": 62, "value": 2 },
            { "source": 66, "target": 65, "value": 2 },
            { "source": 66, "target": 48, "value": 1 },
            { "source": 66, "target": 63, "value": 1 },
            { "source": 66, "target": 61, "value": 1 },
            { "source": 66, "target": 60, "value": 1 },
            { "source": 67, "target": 57, "value": 3 },
            { "source": 68, "target": 25, "value": 5 },
            { "source": 68, "target": 11, "value": 1 },
            { "source": 68, "target": 24, "value": 1 },
            { "source": 68, "target": 27, "value": 1 },
            { "source": 68, "target": 48, "value": 1 },
            { "source": 68, "target": 41, "value": 1 },
            { "source": 69, "target": 25, "value": 6 },
            { "source": 69, "target": 68, "value": 6 },
            { "source": 69, "target": 11, "value": 1 },
            { "source": 69, "target": 24, "value": 1 },
            { "source": 69, "target": 27, "value": 2 },
            { "source": 69, "target": 48, "value": 1 },
            { "source": 69, "target": 41, "value": 1 },
            { "source": 70, "target": 25, "value": 4 },
            { "source": 70, "target": 69, "value": 4 },
            { "source": 70, "target": 68, "value": 4 },
            { "source": 70, "target": 11, "value": 1 },
            { "source": 70, "target": 24, "value": 1 },
            { "source": 70, "target": 27, "value": 1 },
            { "source": 70, "target": 41, "value": 1 },
            { "source": 70, "target": 58, "value": 1 },
            { "source": 71, "target": 27, "value": 1 },
            { "source": 71, "target": 69, "value": 2 },
            { "source": 71, "target": 68, "value": 2 },
            { "source": 71, "target": 70, "value": 2 },
            { "source": 71, "target": 11, "value": 1 },
            { "source": 71, "target": 48, "value": 1 },
            { "source": 71, "target": 41, "value": 1 },
            { "source": 71, "target": 25, "value": 1 },
            { "source": 72, "target": 26, "value": 2 },
            { "source": 72, "target": 27, "value": 1 },
            { "source": 72, "target": 11, "value": 1 },
            { "source": 73, "target": 48, "value": 2 },
            { "source": 74, "target": 48, "value": 2 },
            { "source": 74, "target": 73, "value": 3 },
            { "source": 75, "target": 69, "value": 3 },
            { "source": 75, "target": 68, "value": 3 },
            { "source": 75, "target": 25, "value": 3 },
            { "source": 75, "target": 48, "value": 1 },
            { "source": 75, "target": 41, "value": 1 },
            { "source": 75, "target": 70, "value": 1 },
            { "source": 75, "target": 71, "value": 1 },
            { "source": 76, "target": 64, "value": 1 },
            { "source": 76, "target": 65, "value": 1 },
            { "source": 76, "target": 66, "value": 1 },
            { "source": 76, "target": 63, "value": 1 },
            { "source": 76, "target": 62, "value": 1 },
            { "source": 76, "target": 48, "value": 1 },
            { "source": 76, "target": 58, "value": 1 }
        ]
    };
});

ke_app.controller('PaperListCtrl', function ($scope, $rootScope, $http) {
    $scope.addQueryString = '';



    $scope.items = [];
    $scope.currPaper = {
        "title": "Chemical intervention in plant sugar signalling increases yield and resilience",
        "authorsList": [
            {
                "familyName": "Paul",
                "givenName": "Matthew J.",
                "refAbout": "http://id.crossref.org/contributor/matthew-j-paul-2kz2ky69y7u27",
                "msId": -1
            },
            {
                "familyName": "Sagar",
                "givenName": "Ram",
                "refAbout": "http://id.crossref.org/contributor/ram-sagar-2kz2ky69y7u27",
                "msId": -1
            },
            {
                "familyName": "Patel",
                "givenName": "Mitul K.",
                "refAbout": "http://id.crossref.org/contributor/mitul-k-patel-2kz2ky69y7u27",
                "msId": -1
            },
            {
                "familyName": "Passarelli",
                "givenName": "Melissa K.",
                "refAbout": "http://id.crossref.org/contributor/melissa-k-passarelli-2kz2ky69y7u27",
                "msId": -1
            },
            {
                "familyName": "Griffiths",
                "givenName": "Cara A.",
                "refAbout": "http://id.crossref.org/contributor/cara-a-griffiths-2kz2ky69y7u27",
                "msId": -1
            },
            {
                "familyName": "Bunch",
                "givenName": "Josephine",
                "refAbout": "http://id.crossref.org/contributor/josephine-bunch-2kz2ky69y7u27",
                "msId": -1
            },
            {
                "familyName": "Gilmore",
                "givenName": "Ian S.",
                "refAbout": "http://id.crossref.org/contributor/ian-s-gilmore-2kz2ky69y7u27",
                "msId": -1
            },
            {
                "familyName": "Geng",
                "givenName": "Yiqun",
                "refAbout": "http://id.crossref.org/contributor/yiqun-geng-2kz2ky69y7u27",
                "msId": -1
            },
            {
                "familyName": "Primavesi",
                "givenName": "Lucia F.",
                "refAbout": "http://id.crossref.org/contributor/lucia-f-primavesi-2kz2ky69y7u27",
                "msId": -1
            }
        ],
        "journal": {
            "name": "Nature",
            "refAbout": "http://id.crossref.org/issn/0028-0836",
            "msId": -1
        },
        "doi": "10.1038/nature20591",
        "volume": "540",
        "startPage": "574",
        "endPage": "578",
        "pubDate": "2016-12-14",
        "Abstract": "",
        "Keywords": [],
        "refAbout": "http://dx.doi.org/10.1038/nature20591",
        "Note": "",
        "msId": -1
    };
    $scope.currAbstract = "";
    $scope.currKeywords = "";
    $scope.currNote = "";
    $scope.currWebpage = {
        "title": "Japan passes landmark bill for Emperor Akihito to abdicate - BBC News",
        "author": null,
        "date_published": null,
        "dek": null,
        "lead_image_url": "https://ichef-1.bbci.co.uk/news/1024/cpsprodpb/3FBC/production/_96361361_7e618eba-8f40-46d2-b0ba-ef1a187ed49f.jpg",
        "content": "<div class=\"story-body__inner\"> <figure class=\"media-landscape no-caption full-width lead\"> <span class=\"image-and-copyright-container\"> <img class=\"js-image-replace\" alt=\"Japanese Emperor Akihito waves to well-wishers during his new year speech on the balcony of the Imperial Palace in Tokyo on 2 January 2017.\" src=\"https://ichef.bbci.co.uk/news/320/cpsprodpb/3FBC/production/_96361361_7e618eba-8f40-46d2-b0ba-ef1a187ed49f.jpg\" width=\"976\"> <span class=\"off-screen\">Image copyright</span> <span class=\"story-image-copyright\">AFP</span> </span> </figure><p class=\"story-body__introduction\">Japan&apos;s parliament has passed a one-off bill to allow Emperor Akihito to abdicate, the first emperor to do so in 200 years. </p><p>The 83-year-old said last year that his age and health were making it hard for him to fulfil his official duties.</p><p>But there was no provision under existing law for him to stand down.</p><p>The government will now begin the process of arranging his abdication, expected to happen in late 2018, and the handover to Crown Prince Naruhito.</p><p>Akihito, who has had heart surgery and was treated for prostate cancer, has been on the throne in Japan since the death of his father, Hirohito, in 1989.</p><p>In a <a href=\"http://www.bbc.co.uk/news/world-asia-37007106\" class=\"story-body__link\">rare address to the nation last year</a>, he said he was beginning to feel &quot;constraints&quot; on his health which were making it hard for him to fulfil his official duties. </p><p>The emperor is constitutionally barred from making any political statements, so he could not say explicitly that he wanted to stand down as that would be considered comment on the law.</p><figure class=\"media-landscape has-caption full-width\"> <figcaption class=\"media-caption\"> <span class=\"off-screen\">Image caption</span> <span class=\"media-caption__text\"> Crown Prince Naruhito (third from left) is first in line to take the throne after his father abdicates </span> </figcaption> </figure><p>The newly passed law says that on abdication, the emperor&apos;s 57-year old son, Naruhito, will immediately take the Chrysanthemum Throne, but that neither he nor his successors would be allowed to abdicate under the law.</p><p>The government is yet to set a date for the abdication, but the bill says it must take place within three years of the law coming into effect. </p><p>The handover is widely expected take place in December 2018. </p><p><strong>What does the emperor do? </strong>The emperor has no political powers but several official duties, such as greeting foreign dignitaries. Japan&apos;s monarchy is entwined in the Shinto religion and the emperor still performs religious ceremonies.</p><p><strong>What do the public think?</strong> Most support the emperor&apos;s desire to abdicate - a survey by the Kyodo news agency after Akihito suggested he wanted to step down found more than 85% saying abdication should be legalised.</p><p><strong>Are there more debates revision of the law of royal succession? </strong>A discussion about whether or not a woman would be able to ascend the throne was triggered in 2006 when the emperor had no grandsons, but was postponed after a boy was born to the imperial family. </p> </div>",
        "next_page_url": null,
        "url": "http://www.bbc.com/news/world-asia-40168983",
        "domain": "www.bbc.com",
        "excerpt": "Emperor Akihito is expected to step down in 2018, and be succeeded by Crown Prince Naruhito.",
        "word_count": 380,
        "direction": "ltr",
        "total_pages": 1,
        "rendered_pages": 1,
        "illusts": [{
            "imgurl": "http://testimgurl/testimg.jpg",
            "tags": ["testtag"],
            "format": "jpeg",
            "width": 400,
            "height": 400
        }],
        "tags": ["Akihito", "Japan", "Emperor_of_Japan", "Naruhito"]
    }
    $scope.isEditing = false;
    $scope.toggleEditing = function () {
        $scope.isEditing = !$scope.isEditing;
        console.log("Toggle editing now...");
        console.log($scope.isEditing);
    }

    $scope.changeNote = function () {
        $http.get("/Home/AddNoteToPaper?newNote=" + $scope.currNote + "&doi=" + $scope.currPaper.doi)
            .then(function successCallback(response) {
                $scope.toggleEditing();
                $scope.refreshList();
            }, function errorCallback(response) {
            });
    }

    $scope.isShowingRef = false;
    $scope.isShowingCited = false;
    $scope.isShowingWebTag = false;
    $scope.isShowingKwd = false;
    $scope.isAddingItem = false;
    $scope.currTag = "";

    $scope.showWebTag = function (tag) {
        $scope.isShowingWebTag = true;
        $scope.searchString = tag;
        $scope.currTag = tag;
    };

    $scope.showKwd = function (tag) {
        $scope.isShowingKwd = true;
        $scope.searchString = tag;
        $scope.currTag = tag;
    }

    $scope.searchString = "";
    $scope.getItemsList = function () {
        $http.get("/Home/List/")
            .then(function successCallback(response) {
                $scope.isShowingRef = false;
                $scope.isShowingCited = false;
                $scope.items = response.data;
                $scope.getThePaper($scope.items[0].doi);
                $scope.searchString = "";
                $scope.addQueryString = "";
                $scope.isShowingRef = false;
                $scope.isShowingCited = false;
                $scope.isShowingWebTag = false;
                $scope.isShowingKwd = false;
                $scope.currTag = "";
                $scope.isAddingItem = false;
            }, function errorCallback(response) {
            });
    }

    $scope.getWebItemsList = function () {
        $http.get("/Home/WebpageList/")
            .then(function successCallback(response) {
                $scope.isShowingRef = false;
                $scope.isShowingCited = false;
                $scope.items = response.data;
                $scope.getThePage($scope.items[0].attr.url);
                $scope.searchString = "";
                $scope.addQueryString = "";
                $scope.isShowingRef = false;
                $scope.isShowingCited = false;
                $scope.isShowingWebTag = false;
                $scope.isShowingKwd = false;
                $scope.currTag = "";
                $scope.isAddingItem = false;
            }, function errorCallback(response) {
            });
    }

    $scope.refreshList = function () {
        if ($rootScope.nowActBtn == 0) {
            $scope.getWebItemsList();
        }
        if ($rootScope.nowActBtn == 1) {
            $scope.getItemsList();
        }
    }

    $scope.getThePaper = function (doi) {
        $http.get("/Home/GetPaper?doi=" + doi)
            .then(function successCallback(response) {
                $scope.currPaper.Abstract = "";
                $scope.currPaper.Keywords = [];
                $scope.currPaper = response.data;
                $scope.currNote = $scope.currPaper.Note;
                console.log(angular.toJson($scope.currPaper));
            }, function errorCallback(response) {
            });
    }

    $scope.getThePage = function (url) {
        $http.get("/Home/GetPage?url=" + url)
            .then(function successCallback(response) {
                $scope.currWebpage = response.data;
                console.log(angular.toJson($scope.currWebpage));
            }, function errorCallback(response) {
            });
    }

    $scope.addItem = function () {
        console.log($scope.addQueryString);
        $http.get("/Home/AddItem?addStr=" + $scope.addQueryString)
            .then(function successCallback(response) {
                $scope.getItemsList();
                $scope.isAddingItem = true;
                $('#myModal').modal('hide');
            });
    }

    $scope.goMsAcademic = function (doi) {
        console.log($scope.addQueryString);
        $http.get("/Home/GoMsAcademic?doi=" + doi)
            .then(function successCallback(response) {
                console.log('Go Microsoft Academic Done!');
            });
    }

    $scope.goPartMsAcademic = function (doi) {
        console.log($scope.addQueryString);
        $http.get("/Home/GoPartMsAcademic?doi=" + doi)
            .then(function successCallback(response) {
                console.log('Go Part Microsoft Academic Done!');
            });
    }

    $scope.deleteItem = function (doi) {
        console.log($scope.currPaper.doi);
        $http.get("/Home/DeleteItem?doi=" + $scope.currPaper.doi)
            .then(function successCallback(response) {
                $scope.getItemsList();
            });
    }

    $scope.deletePage = function (url) {
        console.log($scope.currWebpage.url);
        $http.get("/Home/DeletePage?uri=" + $scope.currWebpage.url)
            .then(function successCallback(response) {
                $scope.getWebItemsList();
            });
    }

    $scope.postItem = function (thePaper) {
        $http.post('/Home/Add', thePaper, {
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            ;
        });
    }

    $scope.getRefList = function (doi) {
        $http.get("/Home/RefList?doi=" + doi)
            .then(function successCallback(response) {
                $scope.isShowingRef = true;
                $scope.items = response.data;
                $scope.getThePaper($scope.items[0].doi);
                $scope.addQueryString = "";
            }, function errorCallback(response) {
            });
    }

    $scope.getCitedBy = function (doi) {
        $http.get("/Home/CiteList?doi=" + doi)
            .then(function successCallback(response) {
                $scope.isShowingCited = true;
                $scope.items = response.data;
                $scope.getThePaper($scope.items[0].doi);
                $scope.addQueryString = "";
            }, function errorCallback(response) {
            });
    }


    $scope.showGraph = function () {
        if ($rootScope.nowActBtn == 1) {
            $scope.getGraph('MATCH path = (q:Keyword)-->(n:Paper)-[r]->(m) RETURN path');
            $rootScope.nowActBtn = 2;
        }
        if ($rootScope.nowActBtn == 0) {
            $scope.getGraph('MATCH path = (n:WebPage)-[r]->(m) RETURN path');
            $rootScope.nowActBtn = 2;
        }
    }

    $scope.getGraph = function (cypherState) {
        var username = 'neo4j';
        var password = '19961117JC';
        var headers = { authorization: "Basic " + btoa(username + ":" + password) };
        var res = {};
        var cypherReq = {
            "statements": [{
                "statement": "",
                "resultDataContents": ["graph"]
            }]
        };
        cypherReq.statements[0].statement = cypherState;
        console.log(cypherState);
        console.log(angular.toJson(cypherReq));

        $http({
            method: "POST",
            url: "http://localhost:7474/db/data/transaction/commit",
            data: angular.toJson(cypherReq),
            headers: headers
        }).then(function successCallback(response) {
            res = response.data;
            console.log(angular.toJson(res));

            function idIndex(a, id) {
                for (var i = 0; i < a.length; i++) {
                    if (a[i].id == id) return i;
                }
                return null;
            }

            function getGroup(type) {
                if (type == 'Author') {
                    return 1;
                }
                if (type == 'Paper') {
                    return 2;
                }
                if (type == 'Journal') {
                    return 4;
                }
                if (type == 'Keyword') {
                    return 9;
                }
                if (type == 'WebPage') {
                    return 3;
                }
                if (type == 'WikiTag') {
                    return 1;
                }
            }

            function getTitle(n) {
                if (n.labels[0] == 'Author') {
                    return n.properties.givenName + " " + n.properties.familyName;
                }
                if (n.labels[0] == 'Paper') {
                    return n.properties.title;
                }
                if (n.labels[0] == 'Journal') {
                    return n.properties.name;
                }
                if (n.labels[0] == 'Keyword') {
                    return n.properties.name;
                }
                if (n.labels[0] == 'WebPage') {
                    return n.properties.title;
                }
                if (n.labels[0] == 'WikiTag') {
                    return n.properties.WikiPediaID;
                }

            }

            function getShow(n) {
                if (n.labels[0] == 'Author') {
                    return n.properties.familyName;
                }
                if (n.labels[0] == 'Paper') {
                    return '';
                }
                if (n.labels[0] == 'Journal') {
                    return n.properties.name;
                }
                if (n.labels[0] == 'Keyword') {
                    return n.properties.name;
                }
                if (n.labels[0] == 'WebPage') {
                    return '';
                }
                if (n.labels[0] == 'WikiTag') {
                    return n.properties.WikiPediaID;
                }

            }

            var nodes = [], links = [];
            res.results[0].data.forEach(function (row) {
                row.graph.nodes.forEach(function (n) {
                    if (idIndex(nodes, n.id) == null)
                        nodes.push({ id: n.id, label: n.labels[0], show: getShow(n), title: getTitle(n), group: getGroup(n.labels[0]) });
                });
                links = links.concat(row.graph.relationships.map(function (r) {
                    return { source: idIndex(nodes, r.startNode), target: idIndex(nodes, r.endNode), weight: 5 };
                }));
            });
            viz = { nodes: nodes, links: links };
            console.log(angular.toJson(viz));
            $rootScope.data = viz;
        }, function errorCallback(response) {
        });
    }

    $scope.$on('buttonChanged', function () {
        console.log('Button Changed!!!');
        $scope.refreshList();
    });
});