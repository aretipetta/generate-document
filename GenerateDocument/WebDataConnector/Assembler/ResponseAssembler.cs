using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDocument.WebDataConnector.Assembler
{
    class ResponseAssembler
    {

        public CustomResponseFromFBDB selectExercisesByMuscleGroupForDescription(FirebaseResponse firebaseResponse, String bodyCategory, String muscleGroup)
        {
            if(!firebaseResponse.StatusCode.ToString().Equals("OK"))
            {
                return new CustomResponseFromFBDB(false, "Could not fetch any exercise from database.");
            }
            // else if response == OK
            /*CustomResponseFromFBDB response = new CustomResponseFromFBDB();
            response.OK = true;
            // from dict get the list that is needed
            response.ResponseBody = toExercisesList(firebaseResponse.ResultAs<Dictionary<String, ExerciseRecordFBDB>>());
            // create timer before we send results to be sure that we have them (?)
            return response;
            */
            Dictionary<String, ExerciseRecordFBDB> dict = firebaseResponse.ResultAs<Dictionary<String, ExerciseRecordFBDB>>();
            // the above dictionary contains:
            // key=randomId,
            // value=ExerciseRecordFBDB instance
            // So, filter the values of the dict to get only the exercises where muscleGroup == needed muscleGroup (for the comboBox for 'description')
            List<ExerciseRecordFBDB> myList = dict.Values.Where(x => x.MuscleGroup.Equals(bodyCategory)).ToList();
            return new CustomResponseFromFBDB(true, toExercisesList(firebaseResponse.ResultAs<Dictionary<String, ExerciseRecordFBDB>>()));
        }

        private List<ExerciseRecordFBDB> toExercisesList(Dictionary<String, ExerciseRecordFBDB> dictionaryResults)
        {
            List<ExerciseRecordFBDB> listOfExercises = new List<ExerciseRecordFBDB>();
            // for the following dictionary:
            // key == the random id-key in firebase
            // value == ExerciseRecordFBDB instance
            foreach (var item in dictionaryResults)
            {
                listOfExercises.Add(item.Value);
            }
            return listOfExercises;
        }

        public CustomResponseFromFBDB selectMuscleGroupsByCategoryToCustomResponse(FirebaseResponse firebaseResponse, String bodyCategory)
        {
            if (!firebaseResponse.StatusCode.ToString().Equals("OK"))
            {
                return new CustomResponseFromFBDB(false, "Could not fetch any exercise from database.");
            }
            // else... return all the records for this bodyCategory
            return new CustomResponseFromFBDB(true, toExercisesList(firebaseResponse.ResultAs<Dictionary<String, ExerciseRecordFBDB>>()));
        }

        public CustomResponseFromFBDB insertNewRecordToDB(FirebaseResponse results, String bodyCategory)
        {
            if(!results.StatusCode.ToString().Equals("OK"))
            {
                return new CustomResponseFromFBDB(false, "Could not add any exercise to database.");
            }
            return new CustomResponseFromFBDB(true, "New exercise added successfully");
        }
    }
}
