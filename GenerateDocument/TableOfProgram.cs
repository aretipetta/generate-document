using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDocument
{
    public class TableOfProgram
    {
        private CategoryEnum category;
        // exercises: contains all the records of a table of program
        private System.ComponentModel.BindingList<Exercise> exercises;

        public TableOfProgram(CategoryEnum category)
        {
            this.category = category;
            this.Exercises = new System.ComponentModel.BindingList<Exercise>();
        }

        public CategoryEnum Category { get => category; set => category = value; }
        internal System.ComponentModel.BindingList<Exercise> Exercises { get => exercises; set => exercises = value; }

        public void addExercise(String muscleGroup, String description, String equipment, int set, int reps, int rest, String notes)
        {
            Exercises.Add(new Exercise(muscleGroup, description, equipment, set, reps, rest, notes));
        }
    }
}
