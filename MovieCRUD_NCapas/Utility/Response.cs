﻿namespace MovieCRUD_NCapas.Utility
{
    public class Response<T>
    {
        public bool status {  get; set; }
        public T? value { get; set; }
        public string? msg { get; set; }
    }
}
