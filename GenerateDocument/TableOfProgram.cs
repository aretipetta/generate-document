using System;

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

        public String[] listToRowVector()
        {
            String[] items = new String[Enum.GetValues(typeof(ColumnEnum)).Length * exercises.Count];
            int offset = Enum.GetValues(typeof(ColumnEnum)).Length;
            int indexOfList = 0;
            foreach(Exercise e in exercises)
            {
                items[offset * indexOfList] = e.MuscleGroup;
                items[(offset * indexOfList) + 1] = e.Description;
                items[(offset * indexOfList) + 2] = e.Equipment;
                items[(offset * indexOfList) + 3] = e.Set.ToString();
                items[(offset * indexOfList) + 4] = e.Reps.ToString();
                items[(offset * indexOfList) + 5] = e.Rest.ToString();
                items[(offset * indexOfList) + 6] = e.Notes;
                indexOfList++;
            }
            return items;
        }

        public int getTablesSize()
        {
            return exercises.Count;
        }
    }
}
