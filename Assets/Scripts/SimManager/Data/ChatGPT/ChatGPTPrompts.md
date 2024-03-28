# Locations.json 

Generate a small University town on a grid map. List the locations, businesses, parks, and residences that may exist in a small university town, what services they offer (as a list of single words), and their x, y coordinates. All locations must be connected by roads. Roads are a series of connected x, y points on the grid. Return the output as valid JSON. Here's an example of two locations and the road objects that connects them: 

[
     {
         "Name": "Computer Lab",
         "X": 10,
         "Y": 3,
         "Type": "Business",
         "Tags": [
                "has:computer", "internet", "printing"
         ]
  },
 {
         "Name": "Park",
         "X": 10,
         "Y": 6,
         "Type": "Park",
         "Tags": [
               "trails", "waterfall"
         ]
  },
  {
         "Name": "Road",
         "X": 10,
         "Y": 4,
         "Type": "Road",
         "Tags": [
               "Road"
         ]
  },
  {
         "Name": "Road",
         "X": 10,
         "Y": 5,
         "Type": "Road",
         "Tags": [
               "Road"
         ]
  },
]

This JSON represents a small university town with various locations, businesses, parks, residences, and roads connecting them. Each entity is defined with its name, coordinates, type, and tags describing the services or features it offers. Roads connect the different locations on the grid.

<!-- Old Output -->

[
  {
	"Name": "A2",
	"X": 0,
	"Y": 20,
	"Tags": [
		"Building",
		"Restaurant"
	]
  },
  {
    "Name": "A3",
    "X": 0,
    "Y": 19,
    "Tags": [
      "Building",
      "Gym"
    ]
  },
  {
    "Name": "A4",
    "X": 0,
    "Y": 18,
    "Tags": [
      "Forest"
    ]
  },
  {
    "Name": "A5",
    "X": 0,
    "Y": 17,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "A6",
    "X": 0,
    "Y": 16,
    "Tags": [
      "Forest"
    ]
  },
  {
    "Name": "B2",
    "X": 1,
    "Y": 20,
    "Tags": [
      "Building",
      "Store"
    ]
  },
  {
    "Name": "B3",
    "X": 1,
    "Y": 19,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "B4",
    "X": 1,
    "Y": 18,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "B5",
    "X": 1,
    "Y": 17,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "B6",
    "X": 1,
    "Y": 16,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "B7",
    "X": 1,
    "Y": 15,
    "Tags": [
      "Forest"
    ]
  },
  {
    "Name": "C3",
    "X": 2,
    "Y": 19,
    "Tags": [
      "Building",
      "Store"
    ]
  },
  {
    "Name": "C4",
    "X": 2,
    "Y": 18,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "C5",
    "X": 2,
    "Y": 17,
    "Tags": [
      "Building",
      "Store"
    ]
  },
  {
    "Name": "C6",
    "X": 2,
    "Y": 16,
    "Tags": [
      "Forest"
    ]
  },
  {
    "Name": "C20",
    "X": 2,
    "Y": 2,
    "Tags": [
      "Building",
      "has:Computer"
    ]
  },
  {
    "Name": "C21",
    "X": 2,
    "Y": 1,
    "Tags": [
      "Forest"
    ]
  },
  {
    "Name": "D3",
    "X": 3,
    "Y": 19,
    "Tags": [
      "Forest"
    ]
  },
  {
    "Name": "D4",
    "X": 3,
    "Y": 18,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "D5",
    "X": 3,
    "Y": 17,
    "Tags": [
      "Building",
      "Store",
      "has:Employees"
    ]
  },
  {
    "Name": "D19",
    "X": 3,
    "Y": 3,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "D20",
    "X": 3,
    "Y": 2,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "D21",
    "X": 3,
    "Y": 1,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "D22",
    "X": 3,
    "Y": 0,
    "Tags": [
      "Building",
	  "has:Computer"
    ]
  },
  {
    "Name": "E3",
    "X": 4,
    "Y": 19,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "E4",
    "X": 4,
    "Y": 18,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "E5",
    "X": 4,
    "Y": 17,
    "Tags": [
      "Building",
      "has:Employees"
    ]
  },
  {
    "Name": "E6",
    "X": 4,
    "Y": 16,
    "Tags": [
      "Building",
      "Restaurant"
    ]
  },
  {
    "Name": "E7",
    "X": 4,
    "Y": 15,
    "Tags": [
      "Building",
      "Restaurant"
    ]
  },
  {
    "Name": "E8",
    "X": 4,
    "Y": 14,
    "Tags": [
      "Forest"
    ]
  },
  {
    "Name": "E9",
    "X": 4,
    "Y": 13,
    "Tags": [
      "Building",
      "has:Bed"
    ]
  },
  {
    "Name": "E10",
    "X": 4,
    "Y": 12,
    "Tags": [
      "Building",
      "has:Bed",
      "has:Computer"
    ]
  },
  {
    "Name": "E11",
    "X": 4,
    "Y": 11,
    "Tags": [
      "Building",
      "has:Bed"
    ]
  },
  {
    "Name": "E12",
    "X": 4,
    "Y": 10,
    "Tags": [
      "Park"
    ]
  },
  {
    "Name": "E13",
    "X": 4,
    "Y": 9,
    "Tags": [
      "Park"
    ]
  },
  {
    "Name": "E14",
    "X": 4,
    "Y": 8,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "E15",
    "X": 4,
    "Y": 7,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "E16",
    "X": 4,
    "Y": 6,
    "Tags": [
      "Park"
    ]
  },
  {
    "Name": "E17",
    "X": 4,
    "Y": 5,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "E18",
    "X": 4,
    "Y": 4,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "E19",
    "X": 4,
    "Y": 3,
    "Tags": [
      "Building",
      "Gym"
    ]
  },
  {
    "Name": "E20",
    "X": 4,
    "Y": 2,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "E21",
    "X": 4,
    "Y": 1,
    "Tags": [
      "Building",
      "Store"
    ]
  },
  {
    "Name": "F3",
    "X": 5,
    "Y": 19,
    "Tags": [
      "Building",
      "Store"
    ]
  },
  {
    "Name": "F4",
    "X": 5,
    "Y": 18,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "F5",
    "X": 5,
    "Y": 17,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "F6",
    "X": 5,
    "Y": 16,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "F7",
    "X": 5,
    "Y": 15,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "F8",
    "X": 5,
    "Y": 14,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "F9",
    "X": 5,
    "Y": 13,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "F10",
    "X": 5,
    "Y": 12,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "F11",
    "X": 5,
    "Y": 11,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "F12",
    "X": 5,
    "Y": 10,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "F13",
    "X": 5,
    "Y": 9,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "F14",
    "X": 5,
    "Y": 8,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "F15",
    "X": 5,
    "Y": 7,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "F16",
    "X": 5,
    "Y": 6,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "F17",
    "X": 5,
    "Y": 5,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "F18",
    "X": 5,
    "Y": 4,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "F19",
    "X": 5,
    "Y": 3,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "F20",
    "X": 5,
    "Y": 2,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "F21",
    "X": 5,
    "Y": 1,
    "Tags": [
      "Building",
      "Restaurant"
    ]
  },
  {
    "Name": "G3",
    "X": 6,
    "Y": 19,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "G4",
    "X": 6,
    "Y": 18,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "G5",
    "X": 6,
    "Y": 17,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "G6",
    "X": 6,
    "Y": 16,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "G7",
    "X": 6,
    "Y": 15,
    "Tags": [
      "Building",
      "has:Computer"
    ]
  },
  {
    "Name": "G8",
    "X": 6,
    "Y": 14,
    "Tags": [
      "Building",
      "Store"
    ]
  },
  {
    "Name": "G9",
    "X": 6,
    "Y": 13,
    "Tags": [
      "Park"
    ]
  },
  {
    "Name": "G10",
    "X": 6,
    "Y": 12,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "G11",
    "X": 6,
    "Y": 11,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "G12",
    "X": 6,
    "Y": 10,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "G13",
    "X": 6,
    "Y": 9,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "G14",
    "X": 6,
    "Y": 8,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "G15",
    "X": 6,
    "Y": 7,
    "Tags": [
      "Building",
      "Gym"
    ]
  },
  {
    "Name": "G16",
    "X": 6,
    "Y": 6,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "G17",
    "X": 6,
    "Y": 5,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "G18",
    "X": 6,
    "Y": 4,
    "Tags": [
      "Forest"
    ]
  },
  {
    "Name": "G19",
    "X": 6,
    "Y": 3,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "G20",
    "X": 6,
    "Y": 2,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "G21",
    "X": 6,
    "Y": 1,
    "Tags": [
      "Forest"
    ]
  },
  {
    "Name": "H3",
    "X": 7,
    "Y": 19,
    "Tags": [
      "Forest"
    ]
  },
  {
    "Name": "H4",
    "X": 7,
    "Y": 18,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "H5",
    "X": 7,
    "Y": 17,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "H15",
    "X": 7,
    "Y": 7,
    "Tags": [
      "Building",
      "Store"
    ]
  },
  {
    "Name": "H16",
    "X": 7,
    "Y": 6,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "H17",
    "X": 7,
    "Y": 5,
    "Tags": [
      "Building",
      "has:Computer"
    ]
  },
  {
    "Name": "H20",
    "X": 7,
    "Y": 2,
    "Tags": [
      "Forest"
    ]
  },
  {
    "Name": "I2",
    "X": 8,
    "Y": 20,
    "Tags": [
      "Building",
      "Restaurant"
    ]
  },
  {
    "Name": "I3",
    "X": 8,
    "Y": 19,
    "Tags": [
      "Forest"
    ]
  },
  {
    "Name": "I4",
    "X": 8,
    "Y": 18,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "I5",
    "X": 8,
    "Y": 17,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "I15",
    "X": 8,
    "Y": 7,
    "Tags": [
      "Building",
      "has:Computer"
    ]
  },
  {
    "Name": "I16",
    "X": 8,
    "Y": 6,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "I17",
    "X": 8,
    "Y": 5,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "I18",
    "X": 8,
    "Y": 4,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "J1",
    "X": 9,
    "Y": 21,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "J2",
    "X": 9,
    "Y": 20,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "J3",
    "X": 9,
    "Y": 19,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "J4",
    "X": 9,
    "Y": 18,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "J5",
    "X": 9,
    "Y": 17,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "J15",
    "X": 9,
    "Y": 7,
    "Tags": [
      "Forest"
    ]
  },
  {
    "Name": "J16",
    "X": 9,
    "Y": 6,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "J17",
    "X": 9,
    "Y": 5,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "J18",
    "X": 9,
    "Y": 4,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "J19",
    "X": 9,
    "Y": 3,
    "Tags": [
      "Building",
      "Store"
    ]
  },
  {
    "Name": "K2",
    "X": 10,
    "Y": 20,
    "Tags": [
      "Forest"
    ]
  },
  {
    "Name": "K3",
    "X": 10,
    "Y": 19,
    "Tags": [
      "Building",
      "Store"
    ]
  },
  {
    "Name": "K4",
    "X": 10,
    "Y": 18,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "K5",
    "X": 10,
    "Y": 17,
    "Tags": [
      "Building",
      "Restaurant"
    ]
  },
  {
    "Name": "K15",
    "X": 10,
    "Y": 7,
    "Tags": [
      "Forest"
    ]
  },
  {
    "Name": "K16",
    "X": 10,
    "Y": 6,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "K17",
    "X": 10,
    "Y": 5,
    "Tags": [
      "Forest"
    ]
  },
  {
    "Name": "K18",
    "X": 10,
    "Y": 4,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "L3",
    "X": 11,
    "Y": 19,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "L4",
    "X": 11,
    "Y": 18,
    "Tags": [
      "Road"
    ]
  },
  {
    "Name": "L5",
    "X": 11,
    "Y": 17,
    "Tags": [
      "Building"
    ]
  },
  {
    "Name": "L16",
    "X": 11,
    "Y": 6,
    "Tags": [
      "Forest"
    ]
  },
  {
    "Name": "M4",
    "X": 12,
    "Y": 18,
    "Tags": [
      "Building"
    ]
  }
]

Generate a small University town on a grid map. List the locations, businesses, parks, and residences that may exist in a small university town, what services they offer (as a list of single words). Each location must have a unique name, and a unique x, y coordinate. All locations must be connected by roads either directly to the north, south, east or west of them. Longer roads are formed by connected individual road objects  in neighboring x, y coordinates on the grid. Return the output as valid JSON. Here's an example of two locations and the road objects that connects them: 

[
  {
    "Name": "University",
    "X": 5,
    "Y": 5,
    "Type": "University",
    "Tags": ["education", "research", "library"]
  },
  {
    "Name": "Coffee Shop",
    "X": 4,
    "Y": 6,
    "Type": "Business",
    "Tags": ["coffee", "pastries", "study"]
  },
  {
    "Name": "Bookstore",
    "X": 6,
    "Y": 6,
    "Type": "Business",
    "Tags": ["books", "stationery", "magazines"]
  },
  {
    "Name": "Student Housing",
    "X": 4,
    "Y": 4,
    "Type": "Residence",
    "Tags": ["dormitory", "apartments"]
  },
  {
    "Name": "Faculty Housing",
    "X": 6,
    "Y": 4,
    "Type": "Residence",
    "Tags": ["houses", "apartments"]
  },
  {
    "Name": "Main Street Park",
    "X": 5,
    "Y": 7,
    "Type": "Park",
    "Tags": ["picnic", "playground", "benches"]
  },
  {
    "Name": "Library",
    "X": 4,
    "Y": 5,
    "Type": "Facility",
    "Tags": ["books", "study", "quiet"]
  },
  {
    "Name": "Quad",
    "X": 6,
    "Y": 5,
    "Type": "Facility",
    "Tags": ["green space", "outdoor events", "relaxation"]
  },
  {
    "Name": "East-West Road",
    "X": 5,
    "Y": 6,
    "Type": "Road",
    "Tags": ["road", "connects"]
  },
  {
    "Name": "North-South Road",
    "X": 5,
    "Y": 4,
    "Type": "Road",
    "Tags": ["road", "connects"]
  }
]

# Agents.JSON

Generate a list of characters that can populate a small town. Each character must have a unique name. Some characters can optionally have a social relationship with other characters. Relationships have a valence or a strength associated with them. Some characters may optionally have motives to fulfill (chosen from among motives of: physical, emotional, social, financial, and accomplishment). Return the output as valid JSON. Here's an example: 
[
{
    "Name": "Norma",
    "Motives": {
      "accomplishment": 2,
      "emotional": 3,
      "financial": 1,
      "social": 2,
      "physical": 1
    },
    "Relationships": [
      {
        "Type": "friend",
        "With": "Quentin",
        "Valence": 2
      }
    ]
  },
  {
    "Name": "Abnorma",
    "Relationships": [
      {
        "Type": "friend",
        "With": "Quentin",
        "Valence": 2
      },
      {
        "Type": "employee",
        "With": "Abnorma",
        "Valence": 1
      }
    ]
  },
  {
    "Name": "Quentin",
    "Relationships": [
      {
        "Type": "friend",
        "With": "Norma",
        "Valence": 1
      },
      {
        "Type": "romantic-interest",
        "With": "Abnorma",
        "Valence": 3
      }
    ]
  },
]

# Actions.JSON

User
Generate a list of 15 actions that can be undertaken in a small town. Actions must have a unique name, a minimum number of minutes they take to complete, a set of requirements for the action to be fulfilled, and a set of effects of successful actions. Locations have unique names with tags specified. Action requirements can be Location Requirements, where actions can only be undertaken in locations that either have all of the tags specified, have one or more of the tags specified, or have none of the tags specified. Action requirements can be People Requirements, where actions can only be undertaken if a minimum number of people are present, or absent, or if a specific list of people are present, or a specific relationship type that is present or absent. Effects of actions are changes in the motives (physical, financial, emotional, social, accomplishment) of the agent performing them. For instance, eating at a restaurant may increase the physical motive, but decrease the financial motive. Effects can also change the valence of relationships. For instance, eating at a restaurant with a friend increases their "friendship" relationship valence, but the fighting action may reduce the "friendship"  valence. Produce the output as valid JSON. Here's two example actions: 

[{
      "Name": "Eat",
      "MinTime": 40,
      "Requirements": {
        "Location Requirements": [
          {
            "HasAllOf": [
              "Restaurant"
            ],
            "HasOneOrMoreOf": [],
            "HasNoneOf": []
          }
        ],
      },
      "Effects": {
		"MotiveEffects": [
			{
				"MotiveType": "Physical",
				"Delta": 3
			},
			{
				"MotiveType": "Financial",
				"Delta": -1
			}
		]}
    },
   {
      "Name": "Eat with friend",
      "MinTime": 60,
      "Hidden": false,
      "Requirements": {
        "Location Requirements": [
          {
            "HasAllOf": [
              "Restaurant"
            ],
            "HasOneOrMoreOf": [],
            "HasNoneOf": []
          }
        ],
        "People Requirements": [
          {
            "RelationshipsPresent": [
              "Friend"
            ],
            "MinNumPeople": 2,
            "MaxNumPeople": 60,
            "SpecificPeoplePresent": [],
            "SpecificPeopleAbsent": [],
            "RelationshipsAbsent": []
          }
        ]
      },
      "Effects": {
		"RelationshipEffects": [
			{
				"RelationshipType" : "Friend",
				"Delta": 2
			}
		],
		"MotiveEffects": [
			{
				"MotiveType": "Physical",
				"Delta": 2
			},
			{
				"MotiveType": "Social",
				"Delta": 2
			},
			{
				"MotiveType": "Financial",
				"Delta": -1
			}
		]}
    },
	{
		"Name": "Fight",
		"MinTime": 30,
		"Requirements": {
		"LocationRequirements": [],
		"PeopleRequirements": {
			"MinNumPeople": 2
		}
		},
		"Effects": {
		"MotiveEffects": [
			{
          "MotiveType": "Physical",
          "Delta": -4
        },
        {
          "MotiveType": "Emotional",
          "Delta": -3
        },
        {
          "MotiveType": "Social",
          "Delta": -2
        },
        {
          "MotiveType": "Accomplishment",
          "Delta": -2
        }
      ],
      "RelationshipEffects": [
        {
          "RelationshipType": "Friend",
          "Delta": -3
        }
      ]
    }
  },
  {
    "Name": "Attend Concert",
    "MinTime": 120,
    "Requirements": {
      "LocationRequirements": [
        {
          "HasAllOf": [
            "Concert Hall"
          ],
          "HasOneOrMoreOf": [],
          "HasNoneOf": []
        }
      ],
      "PeopleRequirements": {}
    },
    "Effects": {
      "MotiveEffects": [
        {
          "MotiveType": "Social",
          "Delta": 3
        },
        {
          "MotiveType": "Emotional",
          "Delta": 4
        }
      ],
      "RelationshipEffects": []
    }
  },
  {
    "Name": "Read in Park",
    "MinTime": 60,
    "Requirements": {
      "LocationRequirements": [
        {
          "HasAllOf": [
            "Park"
          ],
          "HasOneOrMoreOf": [],
          "HasNoneOf": []
        }
      ],
      "PeopleRequirements": {}
    },
    "Effects": {
      "MotiveEffects": [
        {
          "MotiveType": "Accomplishment",
          "Delta": 2
        },
        {
          "MotiveType": "Emotional",
          "Delta": 3
        }
      ],
      "RelationshipEffects": []
    }
  },
  {
    "Name": "Attend Community Meeting",
    "MinTime": 90,
    "Requirements": {
      "LocationRequirements": [
        {
          "HasAllOf": [
            "Community Center"
          ],
          "HasOneOrMoreOf": [],
          "HasNoneOf": []
        }
      ],
      "PeopleRequirements": {}
    },
    "Effects": {
      "MotiveEffects": [
        {
          "MotiveType": "Social",
          "Delta": 3
        },
        {
          "MotiveType": "Accomplishment",
          "Delta": 2
        }
      ],
      "RelationshipEffects": []
    }
  },
  {
    "Name": "Visit Museum",
    "MinTime": 90,
    "Requirements": {
      "LocationRequirements": [
        {
          "HasAllOf": [
            "Museum"
          ],
          "HasOneOrMoreOf": [],
          "HasNoneOf": []
        }
      ],
      "PeopleRequirements": {}
    },
    "Effects": {
      "MotiveEffects": [
        {
          "MotiveType": "Accomplishment",
          "Delta": 3
        },
        {
          "MotiveType": "Emotional",
          "Delta": 3
        }
      ],
      "RelationshipEffects": []
    }
  },
  {
    "Name": "Dine at Fine Restaurant",
    "MinTime": 60,
    "Requirements": {
      "LocationRequirements": [
        {
          "HasAllOf": ["Fine Dining", "Restaurant"]
        }
      ],
      "PeopleRequirements": {}
    },
    "Effects": {
      "MotiveEffects": [
        {
          "MotiveType": "Physical",
          "Delta": 3
        },
        {
          "MotiveType": "Social",
          "Delta": 2
        },
        {
          "MotiveType": "Financial",
          "Delta": -3
        }
      ],
      "RelationshipEffects": []
    }
  },
  {
    "Name": "Visit Art Gallery",
    "MinTime": 90,
    "Requirements": {
      "LocationRequirements": [
        {
          "HasAllOf": ["Art Gallery"]
        }
      ],
      "PeopleRequirements": {}
    },
    "Effects": {
      "MotiveEffects": [
        {
          "MotiveType": "Emotional",
          "Delta": 4
        },
        {
          "MotiveType": "Accomplishment",
          "Delta": 3
        }
      ],
      "RelationshipEffects": []
    }
  },
  {
    "Name": "Attend Concert",
    "MinTime": 120,
    "Requirements": {
      "LocationRequirements": [
        {
          "HasAllOf": ["Concert Hall"]
        }
      ],
      "PeopleRequirements": {}
    },
    "Effects": {
      "MotiveEffects": [
        {
          "MotiveType": "Emotional",
          "Delta": 5
        },
        {
          "MotiveType": "Social",
          "Delta": 4
        }
      ],
      "RelationshipEffects": []
    }
  },
  {
    "Name": "Shop at Mall",
    "MinTime": 120,
    "Requirements": {
      "LocationRequirements": [
        {
          "HasAllOf": ["Mall"]
        }
      ],
      "PeopleRequirements": {}
    },
    "Effects": {
      "MotiveEffects": [
        {
          "MotiveType": "Financial",
          "Delta": -4
        },
        {
          "MotiveType": "Physical",
          "Delta": 2
        },
        {
          "MotiveType": "Accomplishment",
          "Delta": 2
        }
      ],
      "RelationshipEffects": []
    }
  },
  {
    "Name": "Take a Sightseeing Tour",
    "MinTime": 180,
    "Requirements": {
      "LocationRequirements": [
        {
          "HasAllOf": ["Tourist Attractions"]
        }
      ],
      "PeopleRequirements": {}
    },
    "Effects": {
      "MotiveEffects": [
        {
          "MotiveType": "Emotional",
          "Delta": 3
        },
        {
          "MotiveType": "Social",
          "Delta": 3
        },
        {
          "MotiveType": "Accomplishment",
          "Delta": 4
        }
      ],
      "RelationshipEffects": []
    }
  },
  {
    "Name": "Workout at Gym",
    "MinTime": 60,
    "Requirements": {
      "LocationRequirements": [
        {
          "HasAllOf": ["Gym"]
        }
      ],
      "PeopleRequirements": {}
    },
    "Effects": {
      "MotiveEffects": [
        {
          "MotiveType": "Physical",
          "Delta": 4
        },
        {
          "MotiveType": "Accomplishment",
          "Delta": 2
        }
      ],
      "RelationshipEffects": []
    }
  },
  {
    "Name": "Visit Theater",
    "MinTime": 150,
    "Requirements": {
      "LocationRequirements": [
        {
          "HasAllOf": ["Theater"]
        }
      ],
      "PeopleRequirements": {}
    },
    "Effects": {
      "MotiveEffects": [
        {
          "MotiveType": "Emotional",
          "Delta": 4
        },
        {
          "MotiveType": "Accomplishment",
          "Delta": 3
        }
      ],
      "RelationshipEffects": []
    }
  },
  {
    "Name": "Dine at Food Court",
    "MinTime": 45,
    "Requirements": {
      "LocationRequirements": [
        {
          "HasAllOf": ["Food Court"]
        }
      ],
      "PeopleRequirements": {}
    },
    "Effects": {
      "MotiveEffects": [
        {
          "MotiveType": "Physical",
          "Delta": 2
        },
        {
          "MotiveType": "Social",
          "Delta": 2
        },
        {
          "MotiveType": "Financial",
          "Delta": -2
        }
      ],
      "RelationshipEffects": []
    }
  },
  {
    "Name": "Attend Business Networking Event",
    "MinTime": 120,
    "Requirements": {
      "LocationRequirements": [
        {
          "HasAllOf": ["Business Center"]
        }
      ],
      "PeopleRequirements": {}
    },
    "Effects": {
      "MotiveEffects": [
        {
          "MotiveType": "Social",
          "Delta": 4
        },
        {
          "MotiveType": "Accomplishment",
          "Delta": 3
        }
      ],
      "RelationshipEffects": []
    }
  },
  {
    "Name": "Visit Bookstore",
    "MinTime": 60,
    "Requirements": {
      "LocationRequirements": [
        {
          "HasAllOf": ["Bookstore"]
        }
      ],
      "PeopleRequirements": {}
    },
    "Effects": {
      "MotiveEffects": [
        {
          "MotiveType": "Accomplishment",
          "Delta": 2
        },
        {
          "MotiveType": "Emotional",
          "Delta": 3
        }
      ],
      "RelationshipEffects": []
    }
  }
]