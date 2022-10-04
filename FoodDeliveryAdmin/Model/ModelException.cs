using System;

namespace FoodDeliveryAdmin.Model
{
    public class ModelException : Exception
    {
        public ModelException() { }

        public ModelException(Exception innerException) : base(String.Empty, innerException) { }
    }
}
