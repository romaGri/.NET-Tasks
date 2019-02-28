using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Serialization.Tasks
{
    // TODO : Make Company class xml-serializable using DataContractSerializer 
    // Employee.Manager should be serialized as reference
    // Company class has to be forward compatible with all derived versions

    [KnownType(typeof(Worker))]
    [KnownType(typeof(Manager))]
    [DataContract]
    public class Company : IExtensibleDataObject
    {
        private ExtensionDataObject data;
        public  ExtensionDataObject ExtensionData
        {
            get { return data; }
            set { data = value; }
        }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public IList<Employee> Employee { get; set; }
 
    }

    [DataContract(IsReference = true)]
    public abstract class Employee {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public Manager Manager { get; set; }
    }

    [DataContract]
    public class Worker : Employee {
        public int Salary { get; set; }
    }
    [DataContract]
    public class Manager : Employee {
        public int YearBonusRate { get; set; } 
    }

}
