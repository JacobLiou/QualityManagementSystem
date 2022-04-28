using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.Entity
{
    public class BaseGenerateIdEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Id主键")]
        public virtual long Id { get; set; }

        public BaseGenerateIdEntity()
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId();
        }
    }
}
