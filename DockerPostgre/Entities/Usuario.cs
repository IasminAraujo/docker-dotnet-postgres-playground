namespace DockerPostgre.Entities
{
    public class Usuario : Entity
    {
        protected Usuario()
        {
            
        }
        public Usuario(string nome) => Nome = nome;
        
        public string Nome { get; private set; }
    }
}
