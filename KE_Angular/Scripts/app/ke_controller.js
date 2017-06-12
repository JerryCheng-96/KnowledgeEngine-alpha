var ke_app = angular.module('KEApp', ['ui.router']);

ke_app.controller('TabCtrl', function ($scope, $rootScope, $http) {
    $rootScope.nowActBtn = 0;
    $scope.navBarClick = function (numberBtn) {
        $rootScope.nowActBtn = numberBtn;
        console.log($rootScope.nowActBtn);
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
        "tags": [{
            "tagid": 1,
            "tagname": "testtag",
            "wikititle": "testwikientry"
        }]
    }
    $scope.isEditing = false;
    $scope.toggleEditing = function () {
        $scope.isEditing = !$scope.isEditing;
        console.log("Toggle editing now...");
        console.log($scope.isEditing);
    }

    $scope.searchString = "";
    $scope.getItemsList = function () {
        $http.get("/Home/List/")
            .then(function successCallback(response) {
                $scope.items = response.data;
                $scope.getThePaper($scope.items[0].doi)
            }, function errorCallback(response) {
            });
    }

    $scope.getThePaper = function (doi) {
        $http.get("/Home/GetPaper?doi=" + doi)
            .then(function successCallback(response) {
                $scope.currPaper.Abstract = "";
                $scope.currPaper.Keywords = [];
                $scope.currPaper = response.data;
            }, function errorCallback(response) {
            });
    }

    $scope.addItem = function () {
        console.log($scope.addQueryString);
        $http.get("/Home/AddItem?addStr=" + $scope.addQueryString)
            .then(function successCallback(response) {
                $scope.items = [];
                $scope.getItemsList();
                $scope.currPaper = response.data;
                $('#myModal').modal('hide');
            });
    }

    $scope.deleteItem = function (doi) {
        console.log($scope.currPaper.doi);
        $http.get("/Home/DeleteItem?doi=" + $scope.currPaper.doi)
            .then(function successCallback(response) {
                //$http.get("/Home/List/")
                //    .then(function successCallback(response) {
                //        $scope.items = response.data;
                //    }, function errorCallback(response) {
                //    });
                //$http.get("/Home/GetPaper?doi=" + $scope.items[0].doi)
                //    .then(function successCallback(response) {
                //        $scope.currPaper = response.data;
                //    }, function errorCallback(response) {
                //    });
                $scope.getItemsList();
            });
    }

    $scope.postItem = function (thePaper) {
        $http.post('/Home/Add', thePaper, {
            headers: { 'Content-Type': 'application/json' }
        }).then(function successCallback(response) {
            ;
        });
    }

});