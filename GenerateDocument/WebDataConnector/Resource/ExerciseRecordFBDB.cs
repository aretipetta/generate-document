using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDocument.WebDataConnector
{
    class ExerciseRecordFBDB
    {

        private String bodyCategory, muscleGroup, exerciseName;

        public ExerciseRecordFBDB(String bodyCategory, String muscleGroup, String exerciseName)
        {
            this.bodyCategory = bodyCategory;
            this.muscleGroup = muscleGroup;
            this.exerciseName = exerciseName;
        }

        public String BodyCategory
        {
            get {return bodyCategory;}
            set { bodyCategory = value; }
        }

        public String MuscleGroup
        {
            get { return muscleGroup; }
            set { muscleGroup = value; }
        }

        public String ExerciseName
        {
            get { return exerciseName; }
            set { exerciseName = value; }
        }
    }
}
