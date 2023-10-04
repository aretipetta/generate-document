using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using FireSharp;
using GenerateDocument.WebDataConnector.Assembler;

namespace GenerateDocument.WebDataConnector.Domain
{
    class ConfigFirebase
    {
        IFirebaseClient client;
        private ResponseAssembler responseAssembler;

        IFirebaseConfig ifc = new FirebaseConfig()
        {
            AuthSecret = "KgRJFYSSNMwHLtVAjLZVS8rWhJLlXfawmpBOdDjj",
            BasePath = "https://gendoc-937ef-default-rtdb.firebaseio.com/exercises_csharp"
        };

        
        public ConfigFirebase()
        {
            client = new FirebaseClient(ifc);
            responseAssembler = new ResponseAssembler();
        }

        public CustomResponseFromFBDB addNewExerciseToDB(String bodyCategory, String muscleGroup, String exerciseName)
        {
            ExerciseRecordFBDB rec = new ExerciseRecordFBDB(bodyCategory, muscleGroup, exerciseName);
            var results = client.Push("/" + bodyCategory, rec);
            return responseAssembler.insertNewRecordToDB(results, bodyCategory);
        }

        public CustomResponseFromFBDB selectDescriptionsByMuscleGroup(String bodyCategory, String muscleGroup)
        {
            var results = client.Get("/" + bodyCategory);
            return responseAssembler.selectExercisesByMuscleGroupForDescription(results, bodyCategory, muscleGroup);
            //return results;
            /*Student std = results.ResultAs<Student>();
            return std;*/
        }

        public CustomResponseFromFBDB selectMuscleGroupsByBodyCategory(String bodyCategory)
        {
            var results = client.Get("/" + bodyCategory);
            return responseAssembler.selectMuscleGroupsByCategoryToCustomResponse(results, bodyCategory);
        }

        public FirebaseResponse testResponse()
        {
            var results = client.Get("/" + "UPPER_BODY");
            return results;
        }


    }
}
