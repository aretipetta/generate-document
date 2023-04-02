using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDocument
{
    public class Exercise
    {
        private String muscleGroup, description, equipment, notes;
        private int set, reps, rest;
        public Exercise(String muscleGroup, String description, String equipment, int set, int reps, int rest, String notes)
        {
            this.MuscleGroup = muscleGroup;
            this.Description = description;
            this.Equipment = equipment;
            this.Set = set;
            this.Reps = reps;
            this.Rest = rest;
            this.Notes = notes;
        }

        public string MuscleGroup { get => muscleGroup; set => muscleGroup = value; }
        public string Description { get => description; set => description = value; }
        public string Equipment { get => equipment; set => equipment = value; }
        public string Notes { get => notes; set => notes = value; }
        public int Set { get => set; set => set = value; }
        public int Reps { get => reps; set => reps = value; }
        public int Rest { get => rest; set => rest = value; }
    }
}
