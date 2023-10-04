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

        public CustomResponseFromFBDB resultsFromSelectExerciseToResponse(FirebaseResponse firebaseResponse)
        {
            if(!firebaseResponse.StatusCode.ToString().Equals("OK"))
            {
                return new CustomResponseFromFBDB(false, "Could not fetch any exercise.");
            }
            // else if response == OK
            CustomResponseFromFBDB response = new CustomResponseFromFBDB();
            response.OK = true;
            // from dict get the list that is needed
            response.ResponseBody = toExercisesList(firebaseResponse.ResultAs<Dictionary<String, ExerciseRecordFBDB>>());
            // create timer before we send results to be sure that we have them (?)
            return response;
        }

        private List<ExerciseRecordFBDB> toExercisesList(Dictionary<String, ExerciseRecordFBDB> dictionaryResults)
        {
            List<ExerciseRecordFBDB> listOfExercises = new List<ExerciseRecordFBDB>();
            // for the following dictionary: key == the random id-ky in firebase - value == ExerciseRecordFBDB instance
            foreach (var item in dictionaryResults)
            {
                listOfExercises.Add(item.Value);
            }
            return listOfExercises;
        }
    }
}
